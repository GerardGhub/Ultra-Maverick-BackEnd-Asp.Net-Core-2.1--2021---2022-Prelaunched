using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using MvcTaskManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class StoreDryWarehouseOrderController : Controller
  {

    private ApplicationDbContext db;

    public StoreDryWarehouseOrderController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/dry_wh_orders_checklist_distinct")]

    public async Task<ActionResult> GetDistinctPreparedOrders()
    {

      string DeActivated = "0";


      List<DryWhOrderParent> obj = new List<DryWhOrderParent>();
      var results = (from a in db.Dry_Wh_Order_Parent
                     join b in db.Dry_wh_orders on a.Id equals b.FK_dry_wh_orders_parent_id

                     where
                     a.Id == b.FK_dry_wh_orders_parent_id &&
                     b.is_active.Equals(true) &&
                     a.Is_for_validation.Contains(DeActivated) &&
                     a.Is_approved.Equals(true) &&
                     a.Is_wh_approved.Equals(false) &&
                     a.Is_active.Equals(true) &&
                     a.Is_prepared.Equals(true)
                     || a.Force_prepared_status != null

                     group a by new
                     {
                       a.Id,
                       a.Is_approved_prepa_date,
                       a.Store_name,
                       a.Route,
                       a.Area,
                       a.Fox,
                       a.Category,
                       a.Is_active,
                       b.is_active,
                       TotalItems = b.is_active

                     } into total

                     select new

                     {
                       Id = total.Key.Id,
                       is_approved_prepa_date = total.Key.Is_approved_prepa_date,
                       store_name = total.Key.Store_name,
                       route = total.Key.Route,
                       area = total.Key.Area,
                       fox = total.Key.Fox,
                       category = total.Key.Category,
                       is_active = total.Key.Is_active,
                       total_state_repack = total.Sum(x => Convert.ToInt32(total.Key.TotalItems)),
                       TotalPreparedItems = (from Order in db.Dry_wh_orders
                                             where total.Key.Fox == Order.fox
                                             && total.Key.Id == Order.FK_dry_wh_orders_parent_id
                                             && Order.is_active.Equals(true)
                                             && Order.is_prepared.Equals(true)

                                             select Order).Count()
                                             - (from Order in db.Dry_wh_orders
                                                where total.Key.Fox == Order.fox
                                                && total.Key.Id == Order.FK_dry_wh_orders_parent_id
                                                && Order.is_active.Equals(true)
                                                && Order.is_wh_checker_cancel.Contains("1")

                                                select Order).Count()


                     }



                    );


      return Ok(results);

      //var GetAllPreparedItems = await results.Where(x => x.total_state_repack == x.TotalPreparedItems || x.TotalRejectItems > 0).ToListAsync();

      var GetAllPreparedItems = await results.Where(x => x.total_state_repack == x.TotalPreparedItems).ToListAsync();


 
      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(GetAllPreparedItems);
      });

      if (GetAllPreparedItems.Count() > 0)
      {
        return (result);
      }
      else
      {

        return NoContent();
      }






    }



    [HttpGet]
    [Route("api/dry_wh_orders_checklist_distinct_partial_cancel")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetDistinctPreparedOrderPartialCancel()
    {


      string DeActivated = "0";
      List<DryWhOrderParent> obj = new List<DryWhOrderParent>();
      var results = (from a in db.Dry_Wh_Order_Parent
                     join b in db.Dry_wh_orders on a.Id equals b.FK_dry_wh_orders_parent_id

                     where
                     a.Id == b.FK_dry_wh_orders_parent_id &&
                     b.is_active.Equals(true) &&
                     a.Is_for_validation.Contains(DeActivated) &&
                     a.Is_approved.Equals(true) &&
                     a.Is_wh_approved.Equals(false) &&
                     a.Is_active.Equals(true)
                     //|| a.Is_prepared.Equals(true)
                     && b.is_prepared.Equals(false)
                     || a.Force_prepared_status != null
                     && b.is_wh_checker_cancel != null

                     group a by new
                     {
                       a.Id,
                       a.Is_approved_prepa_date,
                       a.Store_name,
                       a.Route,
                       a.Area,
                       a.Fox,
                       a.Category,
                       a.Is_active,
                       b.is_active,
                       TotalItems = b.is_active

                     } into total

                     select new

                     {
                       Id = total.Key.Id,
                       is_approved_prepa_date = total.Key.Is_approved_prepa_date,
                       store_name = total.Key.Store_name,
                       route = total.Key.Route,
                       area = total.Key.Area,
                       fox = total.Key.Fox,
                       category = total.Key.Category,
                       is_active = total.Key.Is_active,
                       total_state_repack = total.Sum(x => Convert.ToInt32(total.Key.TotalItems)),
                       Total_state_repack_cancelled_qty = (from Order in db.Dry_wh_orders
                                             where total.Key.Fox == Order.fox
                                             && total.Key.Is_approved_prepa_date == Order.is_approved_prepa_date
                                             && total.Key.Store_name == Order.store_name
                                             && total.Key.Route == Order.route
                                             && Order.is_active.Equals(true)
                                             && Order.is_prepared.Equals(false)
                                             && Order.is_wh_checker_cancel != null
                                             select Order).Count()




                     }



                    );


      //return Ok(results);

      //var GetAllPreparedItems = await results.Where(x => x.total_state_repack == x.TotalPreparedItems || x.TotalRejectItems > 0).ToListAsync();

      var GetAllPreparedItems = await results.Where(x => x.Total_state_repack_cancelled_qty != 0).ToListAsync();



      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(GetAllPreparedItems);
      });

      if (GetAllPreparedItems.Count() > 0)
      {
        return (result);
      }
      else
      {

        return NoContent();
      }



    }



    [HttpGet]
    [Route("api/dry_wh_orders_distinct_store_dispatching")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<DryWhOrderParent>> GetDistinctDispatchingOrders()
    {
      //public async Task<List<DryWhOrder>> GetDistinctDispatchingOrders()
      //string DeActivated = "0";
      //List<DryWhOrder> StoreOrderCheckList = await db.Dry_wh_orders
      //.GroupBy(p => new { p.is_approved_prepa_date, p.fox })
      //.Select(g => g.First()).Where(temp => temp.is_active.Equals(true)
      //&& temp.is_for_validation.Contains(DeActivated)
      //&& temp.is_approved != null
      //&& temp.is_prepared.Equals(true)
      //&& temp.is_wh_approved != null
      //|| temp.force_prepared_status != null).ToListAsync();
      //return StoreOrderCheckList;


  


      var forStoreDispathing = await(from p in db.Dry_Wh_Order_Parent
                               join c in db.Dry_wh_orders on p.Id equals c.FK_dry_wh_orders_parent_id into g
                               where p.Is_active.Equals(true)
                               && p.Is_for_validation.Contains("0")
                               && p.Is_approved.Equals(true)
                               && p.Is_prepared.Equals(true)
                               && p.Is_wh_approved.Equals(true)
                               || p.Force_prepared_status != null
                               select new
                               {
                                 p.Id,
                                 p.Is_approved_prepa_date,
                                 p.Approved_preparation,
                                 p.Fox,
                                 p.Store_name,
                                 p.Route,
                                 p.Area,
                                 p.Category,
                                 p.Is_active,
                                 p.Is_for_validation,
                                 p.Is_approved,
                                 p.Is_prepared,
                                 p.Force_prepared_status,
                                 p.Is_wh_approved,
                                 p.Is_wh_approved_by,
                                 p.Is_wh_approved_date,
                                 p.Wh_checker_move_order_no,
                                 Total_state_repack = g.Count(c => c.is_active)
                               }).ToListAsync();


      return Ok(forStoreDispathing);


    }




    [HttpGet]
    [Route("api/store_orders")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetStoreOrders()
    {

      string DeActivated = "0";

      List<DryWhOrderParent> obj = new List<DryWhOrderParent>();
      var results = (from a in db.Dry_Wh_Order_Parent
                       //join b in db.Allocation_Logs on a.primary_id equals b.order_key
                     join b in db.Dry_wh_orders on a.Id equals b.FK_dry_wh_orders_parent_id

                     where
                     a.Id == b.FK_dry_wh_orders_parent_id &&
                     b.is_active.Equals(true) &&
                     a.Is_for_validation.Contains(DeActivated) &&
                     a.Is_approved.Equals(true) &&
                     a.Is_wh_approved.Equals(false) &&
                     a.Is_active.Equals(true) &&
                     a.Is_prepared.Equals(false)

                     || a.Force_prepared_status != null
                     || b.is_wh_checker_cancel.Contains("1")

                     group a by new
                     {
                       a.Id,
                       a.Is_approved_prepa_date,
                       a.Store_name,
                       a.Route,
                       a.Area,
                       a.Fox,
                       a.Category,
                       a.Is_active,
                       TotalItems = b.is_active




                     } into total

                     select new

                     {

                       total.Key.Id,
                       is_approved_prepa_date = total.Key.Is_approved_prepa_date,
                       store_name = total.Key.Store_name,
                       route = total.Key.Route,
                       area = total.Key.Area,
                       fox = total.Key.Fox,
                       category = total.Key.Category,
                       is_active = total.Key.Is_active,
                       TotalItems = total.Sum(x => Convert.ToInt32(total.Key.TotalItems)),
                       TotalPreparedItems = (from Order in db.Dry_wh_orders
                                             where total.Key.Fox == Order.fox
                                             && total.Key.Is_approved_prepa_date == Order.is_approved_prepa_date
                                             && total.Key.Store_name == Order.store_name
                                             && total.Key.Route == Order.route
                                             && Order.is_active.Equals(true)
                                             //&& Order.is_prepared.Equals(true)
                                             && Order.FK_dry_wh_orders_parent_id == total.Key.Id
                                             select Order).Count() - (from Order in db.Dry_wh_orders
                                                                      where total.Key.Fox == Order.fox
                                                                      && total.Key.Is_approved_prepa_date == Order.is_approved_prepa_date
                                                                      && total.Key.Store_name == Order.store_name
                                                                      && total.Key.Route == Order.route
                                                                      && Order.is_active.Equals(true)
                                                                      && Order.is_prepared.Equals(false)
                                                                      && Order.is_wh_checker_cancel.Contains("1")
                                                                      //&& Order.FK_dry_wh_orders_parent_id == total.Key.Id
                                                                      select Order).Count(),
                       TotalRejectItems = (from Order in db.Dry_wh_orders
                                            where total.Key.Fox == Order.fox
                                            && total.Key.Is_approved_prepa_date == Order.is_approved_prepa_date
                                            && total.Key.Store_name == Order.store_name
                                            && total.Key.Route == Order.route
                                            && Order.is_active.Equals(true)
                                            && Order.is_wh_checker_cancel.Contains("1")
                                            //&& Order.FK_dry_wh_orders_parent_id == total.Key.Id
                                            select Order).Count()



                     }



                    );
      //return Ok(results);



      var GetAllPreparedItems = await results.Where(x => x.TotalItems != x.TotalPreparedItems || x.TotalPreparedItems != 0).ToListAsync();



      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(GetAllPreparedItems);
      });

      if (GetAllPreparedItems.Count() > 0)
      {
        return (result);
      }
      else
      {

        return NoContent();
      }




    }







    [HttpGet]
    [Route("api/dry_wh_orders_checklist_distinct_cancelled")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<List<DryWhOrder>> GetDistinctPreparedCancelledOrders()
    {


      string DeActivated = "0";
      List<DryWhOrder> StoreOrderCheckList = await db.Dry_wh_orders
          .GroupBy(p => new { p.is_approved_prepa_date, p.total_state_repack_cancelled_qty })
          .Select(g => g.First()).Where(temp => temp.is_active.Equals(true)
      && temp.is_for_validation.Contains(DeActivated)
      && temp.is_approved != null
      && temp.is_prepared.Equals(true)
      && temp.is_wh_approved == null
      //&& temp.total_state_repack_cancelled_qty != null
      && temp.is_wh_checker_cancel != null || temp.force_prepared_status != null).ToListAsync();
      return StoreOrderCheckList;
    }


    [HttpGet]
    [Route("api/getStoreOrderMaterialCancelled")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<List<DryWhOrder>> GetStoreOrderMaterialPerItems()
    {
      string Activated = "1";

      List<DryWhOrder> StoreOrderCheckList = await db.Dry_wh_orders
      .Where(temp => temp.is_active.Equals(true)
      && temp.is_wh_checker_cancel.Contains(Activated)).ToListAsync();
      return StoreOrderCheckList;

    }







    [HttpGet]
    [Route("api/store_orders/search/{searchtext}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Search(int searchText)
    {


      List<DryWhOrder> projects = null;

      int FK_dry_wh_orders_parent_id = searchText;
      //if (searchBy == "store_name")

      projects = await db.Dry_wh_orders.Where(temp => temp.is_active.Equals(true)
      && temp.FK_dry_wh_orders_parent_id == FK_dry_wh_orders_parent_id
      && temp.is_wh_checker_cancel == null
      && temp.prepared_allocated_qty != null
      ).ToListAsync();


      List<DryWhOrderViewModel> WarehouseStoreOrderContructor = new List<DryWhOrderViewModel>();
      foreach (var project in projects)
      {
        WarehouseStoreOrderContructor.Add(new DryWhOrderViewModel()
        {

          Primary_id = project.primary_id,
          Is_approved_prepa_date = project.is_approved_prepa_date,
          Store_name = project.store_name,
          Route = project.route,
          Area = project.area,
          Category = project.category,
          Is_active = project.is_active.ToString(),
          Is_for_validation = project.is_for_validation,
          Is_approved = project.is_approved,
          Is_prepared = project.is_prepared.ToString(),
          Force_prepared_status = project.force_prepared_status,
          Fox = project.fox,
          Item_code = project.item_code,
          Description = project.description,
          Uom = project.uom,
          Total_state_repack = project.total_state_repack,
          Qty = project.qty,
          Prepared_allocated_qty = project.prepared_allocated_qty,
          Total_state_repack_cancelled_qty = project.total_state_repack_cancelled_qty,
          FK_dry_wh_orders_parent_id = project.FK_dry_wh_orders_parent_id.ToString()




        });
      }

      return Ok(WarehouseStoreOrderContructor);
    }







    [HttpGet]
    //[Route("api/store_orders_partial_cancel/search/{searchby}/{searchtext}/{searchindex}")]
    [Route("api/store_orders_partial_cancel/search/{searchId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> SearchPartialCancel(int searchId)
    {


      List<DryWhOrder> projects = null;


      int FKDryStoreOrderID = searchId;
      //string FoxStoreCode = searchIndex;
      //if (searchBy == "store_name")

      projects = await db.Dry_wh_orders.Where(temp => temp.is_active.Equals(true) && temp.FK_dry_wh_orders_parent_id == FKDryStoreOrderID && temp.is_wh_checker_cancel != null).ToListAsync();


      List<DryWhOrderViewModel> WarehouseStoreOrderContructor = new List<DryWhOrderViewModel>();
      foreach (var project in projects)
      {
        WarehouseStoreOrderContructor.Add(new DryWhOrderViewModel()
        {

          Primary_id = project.primary_id,
          Is_approved_prepa_date = project.is_approved_prepa_date,
          Store_name = project.store_name,
          Route = project.route,
          Area = project.area,
          Category = project.category,
          Is_active = project.is_active.ToString(),
          Is_for_validation = project.is_for_validation,
          Is_approved = project.is_approved,
          Is_prepared = project.is_prepared.ToString(),
          Force_prepared_status = project.force_prepared_status,
          Fox = project.fox,
          Item_code = project.item_code,
          Description = project.description,
          Uom = project.uom,
          Total_state_repack = project.total_state_repack,
          Qty = project.qty,
          Prepared_allocated_qty = project.prepared_allocated_qty,
          Is_wh_checker_cancel_reason = project.is_wh_checker_cancel_reason,
          Is_wh_checker_cancel_by = project.is_wh_checker_cancel_by,
          Is_wh_checker_cancel_date = project.is_wh_checker_cancel_date,
          Total_state_repack_cancelled_qty = project.total_state_repack_cancelled_qty,
          FK_dry_wh_orders_parent_id = project.FK_dry_wh_orders_parent_id.ToString()






        });
      }

      return Ok(WarehouseStoreOrderContructor);
    }




    [HttpPut]
    [Route("api/store_orders")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Put([FromBody] DryWhOrderParent storeOrders)
    {
      DryWhOrderParent existingProject = await db.Dry_Wh_Order_Parent.Where(temp => temp.Id == storeOrders.Id && temp.Is_prepared.Equals(true)).FirstOrDefaultAsync();
      if (existingProject != null)
      {
        existingProject.Is_wh_approved = storeOrders.Is_wh_approved;
        existingProject.Is_wh_approved_by = storeOrders.Is_wh_approved_by;
        existingProject.Is_wh_approved_date = storeOrders.Is_wh_approved_date;
        existingProject.Wh_checker_move_order_no = storeOrders.Wh_checker_move_order_no;



        await db.SaveChangesAsync();

        DryWhOrderParent existingProject2 = await db.Dry_Wh_Order_Parent.Where(temp => temp.Id == storeOrders.Id).FirstOrDefaultAsync();
        DryWhOrderParentViewModel projectViewModel = new DryWhOrderParentViewModel()
        {
          Id = existingProject2.Id,
          Is_approved_prepa_date = existingProject2.Is_approved_prepa_date,
          Approved_preparation = existingProject2.Approved_preparation,
          Store_name = existingProject2.Store_name,
          Route = existingProject2.Route,
          Area = existingProject2.Area,
          Category = existingProject2.Category,
          Is_active = existingProject2.Is_active,
          Fox = existingProject2.Fox,
          Is_wh_approved = existingProject2.Is_wh_approved.ToString(),
          Is_wh_approved_by = existingProject2.Is_wh_approved_by,
          Is_wh_approved_date = existingProject2.Is_wh_approved_date,
          Wh_checker_move_order_no = existingProject2.Wh_checker_move_order_no.ToString()



        };
        return Ok(projectViewModel);
      }
      else
      {
        return null;
      }
    }



    [HttpPut]
    [Route("api/store_orders/cancelitems")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutCancelPreparedItem([FromBody] DryWhOrder project)
    {
      DryWhOrder existingProject = await db.Dry_wh_orders.Where(temp => temp.FK_dry_wh_orders_parent_id == project.FK_dry_wh_orders_parent_id && temp.primary_id == project.primary_id).FirstOrDefaultAsync();
      if (existingProject != null)
      {
        existingProject.is_wh_checker_cancel = project.is_wh_checker_cancel;
        existingProject.is_wh_checker_cancel_by = project.is_wh_checker_cancel_by;
        existingProject.is_wh_checker_cancel_date = project.is_wh_checker_cancel_date;
        existingProject.is_wh_checker_cancel_reason = project.is_wh_checker_cancel_reason;

        await db.SaveChangesAsync();

        DryWhOrder existingProject2 = await db.Dry_wh_orders.Where(temp => temp.FK_dry_wh_orders_parent_id == project.FK_dry_wh_orders_parent_id && temp.primary_id == project.primary_id).FirstOrDefaultAsync();
        DryWhOrderViewModel projectViewModel = new DryWhOrderViewModel()
        {
          Primary_id = existingProject2.primary_id,
          Is_approved_prepa_date = existingProject2.is_approved_prepa_date,
          Store_name = existingProject2.store_name,
          Route = existingProject2.route,
          Area = existingProject2.area,
          Category = existingProject2.category,
          Is_active = existingProject2.is_active.ToString(),
          Item_code = existingProject.item_code,
          Description = existingProject2.description,
          Is_wh_checker_cancel = existingProject2.is_wh_checker_cancel,
          Is_wh_checker_cancel_by = existingProject2.is_wh_checker_cancel_by,
          Is_wh_checker_cancel_date = existingProject2.is_wh_checker_cancel_date,
          Is_wh_checker_cancel_reason = existingProject2.is_wh_checker_cancel_reason



        };
        return Ok(projectViewModel);
      }
      else
      {
        return null;
      }
    }




    [HttpPut]
    [Route("api/store_orders/cancelreturnitemslogistic")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutReturnPreparedItem([FromBody] DryWhOrder project)
    {
      DryWhOrder existingProject = await db.Dry_wh_orders.Where(temp => temp.is_approved_prepa_date == project.is_approved_prepa_date && temp.primary_id == project.primary_id).FirstOrDefaultAsync();
      if (existingProject != null)
      {

        existingProject.logic_return_by = project.logic_return_by;
        existingProject.logic_return_date = project.logic_return_date;
        existingProject.logic_return_reason = project.logic_return_reason;
        existingProject.is_wh_checker_cancel = project.is_wh_checker_cancel;



        await db.SaveChangesAsync();

        DryWhOrder existingProject2 = await db.Dry_wh_orders.Where(temp => temp.is_approved_prepa_date == project.is_approved_prepa_date && temp.primary_id == project.primary_id).FirstOrDefaultAsync();
        DryWhOrderViewModel projectViewModel = new DryWhOrderViewModel()
        {

          Logic_return_by = existingProject2.logic_return_by,
          Logic_return_date = existingProject2.logic_return_date,
          Logic_return_reason = existingProject2.logic_return_reason,
          Is_wh_checker_cancel = existingProject2.is_wh_checker_cancel



        };
        return Ok(projectViewModel);
      }
      else
      {
        return null;
      }
    }




    [HttpPut]
    [Route("api/store_orders/cancelreturnitemslogisticstatecount")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutReturnPreparedItemCountState([FromBody] DryWhOrder project)
    {
      DryWhOrder existingProject = await db.Dry_wh_orders.Where(temp => temp.FK_dry_wh_orders_parent_id == project.FK_dry_wh_orders_parent_id && temp.primary_id == project.primary_id).FirstOrDefaultAsync();
      if (existingProject != null)
      {


        existingProject.total_state_repack_cancelled_qty = project.total_state_repack_cancelled_qty;
        existingProject.is_wh_checker_cancel = null;
        existingProject.is_wh_checker_cancel_by = null;
        existingProject.is_wh_checker_cancel_date = null;
        existingProject.is_wh_checker_cancel_reason = null;
        existingProject.total_state_repack_cancelled_qty = null;


        existingProject.is_prepared = true;
        existingProject.Is_Prepared_Date = DateTime.Now.ToString();

        await db.SaveChangesAsync();

        //Update Preparation Per Item
        var existingDataPrepared = await db.Store_Preparation_Logs.Where(temp => temp.ParentIdentity == project.FK_dry_wh_orders_parent_id).ToListAsync();

        foreach (var item in existingDataPrepared)
        {
          var UpdatePeritem = await db.Store_Preparation_Logs.Where(temp => temp.ParentIdentity == project.FK_dry_wh_orders_parent_id && temp.Order_Source_Key == project.primary_id).FirstOrDefaultAsync();

          if (UpdatePeritem != null)
          {
            UpdatePeritem.Is_Active = true;
          }

        }




        DryWhOrder checkTheDataIfIsPrepared = await db.Dry_wh_orders.Where(temp => temp.is_prepared.Equals(false) && temp.is_active.Equals(true)
        && temp.FK_dry_wh_orders_parent_id == project.FK_dry_wh_orders_parent_id).FirstOrDefaultAsync();
        if (checkTheDataIfIsPrepared != null)
        {

        }
        else
        {
          DryWhOrderParent existingParentData = await db.Dry_Wh_Order_Parent.Where(temp => temp.Id == project.FK_dry_wh_orders_parent_id).FirstOrDefaultAsync();
          if (existingParentData != null)
          {
            existingParentData.Is_prepared = true;
            existingParentData.Is_prepared_date = DateTime.Now.ToString();
          }



        }

        await db.SaveChangesAsync();


        DryWhOrder existingProject2 = await db.Dry_wh_orders
        .Where(temp => temp.FK_dry_wh_orders_parent_id == project.FK_dry_wh_orders_parent_id
        && temp.primary_id == project.primary_id).FirstOrDefaultAsync();
        DryWhOrderViewModel projectViewModel = new DryWhOrderViewModel()
        {
          Total_state_repack_cancelled_qty = existingProject2.total_state_repack_cancelled_qty,
        };
        return Ok(projectViewModel);
      }
      else
      {
        return null;
      }
    }


    [HttpPut]
    [Route("api/store_orders/CancelParentTransaction")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CancelParentTransaction([FromBody] DryWhOrderParent ParentSource)
    {

      var parentData = await db.Dry_Wh_Order_Parent.Where(temp => temp.Id == ParentSource.Id).FirstOrDefaultAsync();
      if (parentData != null)
      {
        parentData.Is_prepared = false;
        parentData.Is_prepared_date = null;
      }
      await db.SaveChangesAsync();


      var preparationLogs = await db.Store_Preparation_Logs .Where(temp => temp.ParentIdentity== ParentSource.Id).ToListAsync();
      foreach (var items in preparationLogs)
      {
        var existingdata = await db.Store_Preparation_Logs.Where(temp => temp.ParentIdentity == ParentSource.Id).FirstOrDefaultAsync();
        if (existingdata != null)
        {
          existingdata.Is_Active = false;
        }
      }
      await db.SaveChangesAsync();

      var data = await db.Dry_wh_orders.Where(temp => temp.FK_dry_wh_orders_parent_id == ParentSource.Id).ToListAsync();



      //return Ok(data);
      //items.Is_cancel_date = DateTime.Now;

      foreach (var items in data)
      {

   
       var existingProject = await db.Dry_wh_orders.Where(temp => temp.primary_id == items.primary_id).FirstOrDefaultAsync();
        if (existingProject != null)
        {
          existingProject.is_wh_checker_cancel = "1";
          existingProject.is_wh_checker_cancel_by = ParentSource.Is_cancel_by;
          existingProject.is_wh_checker_cancel_date = DateTime.Now.ToString();
          existingProject.is_wh_checker_cancel_reason = ParentSource.Is_cancelled_reason;
          existingProject.total_state_repack_cancelled_qty = 1;
          //existingProject.is_prepared = false;
          //existingProject.Is_Prepared_Date = null;
          //existingProject.Is_Prepared_By = null;
          //existingProject.Start_Time_Stamp = null;
          //existingProject.Start_By_User_Id = null;
          //existingProject.End_Time_Stamp_Per_Items = null;
          //existingProject.total_state_repack = "0";

          existingProject.is_prepared = false;
          existingProject.Is_Prepared_Date = null;

          await db.SaveChangesAsync();

        }
        else
        {
          return null;
        }

     

      }
      return Ok(ParentSource);



    }


      [HttpPut]
    [Route("api/store_orders/cancelindividualitems")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutCancelPreparedCancelledCountItem([FromBody] DryWhOrder storeOrders)
    {
      DryWhOrder existingProject = await db.Dry_wh_orders.Where(temp => temp.FK_dry_wh_orders_parent_id == storeOrders.FK_dry_wh_orders_parent_id
      && temp.primary_id == storeOrders.primary_id).FirstOrDefaultAsync();
      if (existingProject != null)
      {
        existingProject.is_wh_checker_cancel = storeOrders.is_wh_checker_cancel;
        existingProject.is_wh_checker_cancel_by = storeOrders.is_wh_checker_cancel_by;
        existingProject.is_wh_checker_cancel_date = storeOrders.is_wh_checker_cancel_date;
        existingProject.is_wh_checker_cancel_reason = storeOrders.is_wh_checker_cancel_reason;
        existingProject.total_state_repack_cancelled_qty = storeOrders.total_state_repack_cancelled_qty;
        existingProject.is_prepared = false;
        existingProject.Is_Prepared_Date = null;
        await db.SaveChangesAsync();


        //Update the Parent Data
        await this.PutCancelledPreparationBulkOnParentTable(storeOrders);


        //Update Preparation Per Item
        await this.PutCancelledStorePreparations(storeOrders);


          DryWhOrder existingProject2 = await db.Dry_wh_orders.Where(temp => temp.FK_dry_wh_orders_parent_id == storeOrders.FK_dry_wh_orders_parent_id && temp.primary_id == storeOrders.primary_id).FirstOrDefaultAsync();
        DryWhOrderViewModel projectViewModel = new DryWhOrderViewModel()
        {
          Primary_id = existingProject2.primary_id,
          Is_approved_prepa_date = existingProject2.is_approved_prepa_date,
          Store_name = existingProject2.store_name,
          Route = existingProject2.route,
          Area = existingProject2.area,
          Category = existingProject2.category,
          Is_active = existingProject2.is_active.ToString(),
          Item_code = existingProject.item_code,
          Description = existingProject2.description,
          Is_wh_checker_cancel = existingProject2.is_wh_checker_cancel,
          Is_wh_checker_cancel_by = existingProject2.is_wh_checker_cancel_by,
          Is_wh_checker_cancel_date = existingProject2.is_wh_checker_cancel_date,
          Is_wh_checker_cancel_reason = existingProject2.is_wh_checker_cancel_reason
        };
        return Ok(projectViewModel);
      }
      else
      {
        return null;
      }
    }


    public async Task PutCancelledPreparationBulkOnParentTable(DryWhOrder storeOrders)
    {
      DryWhOrderParent existingParentData = await db.Dry_Wh_Order_Parent.Where(temp => temp.Id == storeOrders.FK_dry_wh_orders_parent_id).FirstOrDefaultAsync();
      if (existingParentData != null)
      {
        existingParentData.Is_prepared = false;
        existingParentData.Is_prepared_date = null;

      }
      await db.SaveChangesAsync();
    }


    public async Task PutCancelledStorePreparations(DryWhOrder storeOrders)
    {
 
      var UpdatePeritem = await db.Store_Preparation_Logs.Where(temp => temp.ParentIdentity == storeOrders.FK_dry_wh_orders_parent_id && temp.Order_Source_Key == storeOrders.primary_id).ToListAsync();

      foreach (var item in UpdatePeritem)
      {
        if (UpdatePeritem != null)
        {
          item.Is_Active = false;

        }

      }
      await db.SaveChangesAsync();

    }

    [HttpPut]
    [Route("api/store_orders/cancelitems/readline")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutCancelPreparedItemReadLine([FromBody] Store_Preparation_Logs storePreparation)
    {

      List<Store_Preparation_Logs> existingProject = await db.Store_Preparation_Logs.Where(temp => temp.Order_Source_Key == storePreparation.Order_Source_Key ).ToListAsync();
      if (existingProject != null)
      {
        storePreparation.Is_Active = storePreparation.Is_Active;



        await db.SaveChangesAsync();

        List<Store_Preparation_Logs> existingProject2 = await db.Store_Preparation_Logs.Where(temp => temp.Order_Source_Key == storePreparation.Order_Source_Key).ToListAsync();
        Store_Preparation_Logs_View_Model projectViewModel = new Store_Preparation_Logs_View_Model()
        {
          Is_Active = storePreparation.Is_Active.ToString()




        };
        return Ok(projectViewModel);
      }
      else
      {
        return null;
      }
    }



  }
}
