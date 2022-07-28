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
  public class StoreDryWarehouseOrderController : Controller
  {
   
    private ApplicationDbContext db;

    public StoreDryWarehouseOrderController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/dry_wh_orders_checklist_distinct")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> GetDistinctPreparedOrders()
    {
      //string Activated = "1";
      string DeActivated = "0";
      //List<DryWhOrder> StoreOrderCheckList = await db.Dry_wh_orders.GroupBy(p => new {p.is_approved_prepa_date, p.fox}).Select(g => g.First()).Where(temp => temp.is_active.Contains(Activated)
      //&& temp.is_for_validation.Contains(DeActivated)
      //&& temp.is_approved != null
      //&& temp.is_prepared != null
      //&& temp.is_wh_approved == null
      //|| temp.force_prepared_status != null).ToListAsync();
      //return StoreOrderCheckList;


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

                       is_approved_prepa_date = total.Key.Is_approved_prepa_date,
                       store_name = total.Key.Store_name,
                       route = total.Key.Route,
                       area = total.Key.Area,
                       fox = total.Key.Fox,
                       category = total.Key.Category,
                       is_active = total.Key.Is_active,
                       //qty = total.Sum(x => Convert.ToInt32(x.qty)),
                       //prepared_allocated_qty = total.Sum(x => Convert.ToInt32(x.prepared_allocated_qty)),
                       //total_state_repack = total.Sum(x => Convert.ToInt32(x.is_active)),
                       //total_state_repack = total.Sum(x => Convert.ToInt32(x.Is_active)),
                       //total_state_repack = total.Sum(x => Convert.ToInt32(x.is_active)),
                       total_state_repack = total.Sum(x => Convert.ToInt32(total.Key.TotalItems)),
                       //total_state_repack = total.Count(),
                       //total_state_repack = total.Count(),
                       TotalPreparedItems = (from Order in db.Dry_wh_orders
                                             where total.Key.Fox == Order.fox
                                             && total.Key.Is_approved_prepa_date == Order.is_approved_prepa_date
                                             && total.Key.Store_name == Order.store_name
                                             && total.Key.Route == Order.route
                                             && Order.is_active.Equals(true)
                                             && Order.is_prepared != null
                                             select Order).Count()



                     }



                    );


      var GetAllPreparedItems = await results.Where(x => x.total_state_repack == x.TotalPreparedItems).ToListAsync();


      //return Ok(view_trim);
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
    public async Task<List<DryWhOrder>> GetDistinctPreparedOrderPartialCancel()
    {

      string DeActivated = "0";
      List<DryWhOrder> StoreOrderCheckList = await db.Dry_wh_orders.GroupBy(p => new { p.is_approved_prepa_date, p.fox }).Select(g => g.First()).Where(temp => temp.is_active.Equals(true)
        && temp.is_for_validation.Contains(DeActivated)
        && temp.is_approved != null && temp.is_prepared != null
        && temp.is_wh_approved == null
        && temp.is_wh_checker_cancel != null || temp.force_prepared_status != null).ToListAsync();
      return StoreOrderCheckList;


    }



    [HttpGet]
    [Route("api/dry_wh_orders_distinct_store_dispatching")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<List<DryWhOrder>> GetDistinctDispatchingOrders()
    {
      
      string DeActivated = "0";
      List<DryWhOrder> StoreOrderCheckList = await db.Dry_wh_orders.GroupBy(p => new { p.is_approved_prepa_date, p.fox }).Select(g => g.First()).Where(temp => temp.is_active.Equals(true)
      && temp.is_for_validation.Contains(DeActivated)
      && temp.is_approved != null
      && temp.is_prepared != null
      && temp.is_wh_approved != null
      || temp.force_prepared_status != null).ToListAsync();
      return StoreOrderCheckList;

    }




    [HttpGet]
    [Route("api/store_orders")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetStoreOrders()
    {

        string DeActivated = "0";
      //List<DryWhOrder> StoreOrderCheckList = await db.Dry_wh_orders.GroupBy(p => new { p.is_approved_prepa_date }).Select(g => g.First()).Where(temp => temp.is_active.Equals(true)
      //&& temp.is_for_validation.Contains(DeActivated)
      //&& temp.is_approved != null
      //&& temp.is_prepared == null
      //|| temp.force_prepared_status != null).ToListAsync();
      //return StoreOrderCheckList;





 


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

                       Id = total.Key.Id,
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
                                             && Order.is_prepared != null
                                          
                                             select Order).Count()



                     }



                    );




      var GetAllPreparedItems = await results.Where(x => x.TotalItems != x.TotalPreparedItems).ToListAsync();



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
    && temp.is_prepared != null
    && temp.is_wh_approved == null
    && temp.total_state_repack_cancelled_qty != null
    && temp.is_wh_checker_cancel != null || temp.force_prepared_status != null).ToListAsync();
    return StoreOrderCheckList;
    }


    [HttpGet]
    [Route("api/getStoreOrderMaterialCancelled")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<List<DryWhOrder>> GetStoreOrderMaterialPerItems()
    {
      string Activated = "1";
   
      List<DryWhOrder> StoreOrderCheckList = await db.Dry_wh_orders.Where(temp => temp.is_active.Equals(true) && temp.is_wh_checker_cancel.Contains(Activated)).ToListAsync();
      return StoreOrderCheckList;

    }







    [HttpGet]
    [Route("api/store_orders/search/{searchby}/{searchtext}/{searchindex}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Search(string searchBy, string searchText, string searchIndex)
    {


      List<DryWhOrder> projects = null;

      string ApprovedPreparationDate = searchText;
      string FoxStoreCode = searchIndex;
      if (searchBy == "store_name")

        projects = await db.Dry_wh_orders.Where(temp => temp.is_active.Equals(true)
        && temp.is_approved_prepa_date.Contains(ApprovedPreparationDate)
        && temp.fox.Contains(FoxStoreCode)
        && temp.is_wh_checker_cancel == null
        && temp.prepared_allocated_qty != null
        ).ToListAsync();

     
      List <DryWhOrderViewModel> WarehouseStoreOrderContructor = new List<DryWhOrderViewModel>();
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
          Is_prepared = project.is_prepared,
          Force_prepared_status = project.force_prepared_status,
          Fox = project.fox,
          Item_code = project.item_code,
          Description = project.description,
          Uom = project.uom,
          Total_state_repack = project.total_state_repack,
          Qty = project.qty,
          Prepared_allocated_qty = project.prepared_allocated_qty,
          Total_state_repack_cancelled_qty = project.total_state_repack_cancelled_qty
        



        });
      }

      return Ok(WarehouseStoreOrderContructor);
    }


    




    [HttpGet]
    [Route("api/store_orders_partial_cancel/search/{searchby}/{searchtext}/{searchindex}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> SearchPartialCancel(string searchBy, string searchText, string searchIndex)
    {


      List<DryWhOrder> projects = null;

      string ApprovedPreparationDate = searchText;
      string FoxStoreCode = searchIndex;
      if (searchBy == "store_name")

        projects = await db.Dry_wh_orders.Where(temp => temp.is_active.Equals(true) && temp.is_approved_prepa_date.Contains(ApprovedPreparationDate) && temp.fox.Contains(FoxStoreCode) && temp.is_wh_checker_cancel != null).ToListAsync();


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
          Is_prepared = project.is_prepared,
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
          Total_state_repack_cancelled_qty = project.total_state_repack_cancelled_qty






        });
      }

      return Ok(WarehouseStoreOrderContructor);
    }




    [HttpPut]
    [Route("api/store_orders")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Put([FromBody] DryWhOrderParent storeOrders)
    {
      DryWhOrderParent existingProject = await db.Dry_Wh_Order_Parent.Where(temp => temp.Id == storeOrders.Id && temp.Is_prepared.Equals(true) ).FirstOrDefaultAsync();
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
          Is_wh_approved= existingProject2.Is_wh_approved.ToString(),
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
      DryWhOrder existingProject = await db.Dry_wh_orders.Where(temp => temp.is_approved_prepa_date == project.is_approved_prepa_date && temp.primary_id == project.primary_id).FirstOrDefaultAsync();
      if (existingProject != null)
      {


        existingProject.total_state_repack_cancelled_qty = project.total_state_repack_cancelled_qty;




       await db.SaveChangesAsync();

        DryWhOrder existingProject2 = await db.Dry_wh_orders.Where(temp => temp.is_approved_prepa_date == project.is_approved_prepa_date && temp.primary_id == project.primary_id).FirstOrDefaultAsync();
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


        await db.SaveChangesAsync();

        DryWhOrder existingProject2 = await db.Dry_wh_orders.Where(temp => temp.FK_dry_wh_orders_parent_id == storeOrders.FK_dry_wh_orders_parent_id && temp.primary_id == storeOrders.primary_id).FirstOrDefaultAsync();
        DryWhOrderViewModel projectViewModel = new DryWhOrderViewModel()
        {
          Is_wh_checker_cancel = existingProject2.is_wh_checker_cancel,
          Is_wh_checker_cancel_by = existingProject2.is_wh_checker_cancel_by,
          Is_wh_checker_cancel_date = existingProject2.is_wh_checker_cancel_date,
          Is_wh_checker_cancel_reason = existingProject2.is_wh_checker_cancel_reason,
          Total_state_repack_cancelled_qty = existingProject2.total_state_repack_cancelled_qty



        };
        return Ok(projectViewModel);
      }
      else
      {
        return null;
      }
    }


    [HttpPut]
    [Route("api/store_orders/cancelitems/readline")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutCancelPreparedItemReadLine([FromBody] Store_Preparation_LogsModel storePreparation)
    {

      List<Store_Preparation_LogsModel> existingProject = await db.Store_Preparation_Logs.Where(temp => temp.order_source_key == storePreparation.order_source_key ).ToListAsync();
      if (existingProject != null)
      {
        storePreparation.is_active = storePreparation.is_active;



        await db.SaveChangesAsync();

        List<Store_Preparation_LogsModel> existingProject2 = await db.Store_Preparation_Logs.Where(temp => temp.order_source_key == storePreparation.order_source_key).ToListAsync();
        Store_Preparation_Logs_View_Model projectViewModel = new Store_Preparation_Logs_View_Model()
        {
          Is_active = storePreparation.is_active




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
