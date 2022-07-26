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
  public class DryWarehouseOrderController : Controller
  {
   
    private ApplicationDbContext db;

    public DryWarehouseOrderController(ApplicationDbContext db)
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



      List<DryWhOrder> obj = new List<DryWhOrder>();
      var results = (from a in db.Dry_wh_orders
                     join b in db.Allocation_Logs on a.primary_id equals b.order_key
               
                     where
                     a.is_for_validation.Contains(DeActivated) &&
                     a.is_approved != null &&
                     a.is_wh_approved == null &&
                     a.is_active.Equals(true)
                     || a.force_prepared_status != null
                     //&& a.is_prepared != null
                     //&& b.is_active.Equals(true) && c.is_active.Equals(true)

                     //&& a.user_id == user_id

                     group a by new
                     {
                       a.is_approved_prepa_date,
                       a.store_name,
                       a.route,
                       a.area,
                       a.fox,                  
                       a.category,
                       a.is_active
            



                     } into total

                     select new
             
                     {

                       is_approved_prepa_date = total.Key.is_approved_prepa_date,
                       store_name = total.Key.store_name,
                       route = total.Key.route,
                       area = total.Key.area,
                       fox = total.Key.fox,
                       category = total.Key.category,
                       is_active = total.Key.is_active,
                       qty = total.Sum(x => Convert.ToInt32(x.qty)),
                       prepared_allocated_qty = total.Sum(x => Convert.ToInt32(x.prepared_allocated_qty)),
                       total_state_repack = total.Count(),
                  
                       TotalPreparedItems = ( from Order in db.Dry_wh_orders
                                             where total.Key.fox == Order.fox
                                             && total.Key.is_approved_prepa_date == Order.is_approved_prepa_date
                                             && total.Key.store_name == Order.store_name
                                             && total.Key.route == Order.route
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
      string Activated = "1";
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

 



      List<DryWhOrder> obj = new List<DryWhOrder>();
      var results = (from a in db.Dry_wh_orders
                     join b in db.Allocation_Logs on a.primary_id equals b.order_key

                     where
                     a.is_for_validation.Contains(DeActivated) &&
                     a.is_approved != null &&
                     a.is_wh_approved == null &&
                     a.is_active.Equals(true)
                     || a.force_prepared_status != null
                     //&& a.is_prepared != null
                     //&& b.is_active.Equals(true) && c.is_active.Equals(true)

                     //&& a.user_id == user_id

                     group a by new
                     {
                       a.is_approved_prepa_date,
                       a.store_name,
                       a.route,
                       a.area,
                       a.fox,
                       a.category,
                       a.is_active




                     } into total

                     select new

                     {

                       is_approved_prepa_date = total.Key.is_approved_prepa_date,
                       store_name = total.Key.store_name,
                       route = total.Key.route,
                       area = total.Key.area,
                       fox = total.Key.fox,
                       category = total.Key.category,
                       is_active = total.Key.is_active,
                       qty = total.Sum(x => Convert.ToInt32(x.qty)),
                       prepared_allocated_qty = total.Sum(x => Convert.ToInt32(x.prepared_allocated_qty)),
                       TotalItems = total.Count(),
                       TotalPreparedItems = (from Order in db.Dry_wh_orders
                                             where total.Key.fox == Order.fox
                                             && total.Key.is_approved_prepa_date == Order.is_approved_prepa_date
                                             && total.Key.store_name == Order.store_name
                                             && total.Key.route == Order.route
                                             && Order.is_active.Equals(true)
                                             && Order.is_prepared != null
                                             select Order).Count()



                     }



                    );


      var GetAllPreparedItems = await results.Where(x => x.TotalItems != x.TotalPreparedItems).ToListAsync();


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
    [Route("api/dry_wh_orders_checklist_distinct_cancelled")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<List<DryWhOrder>> GetDistinctPreparedCancelledOrders()
    {

    string Activated = "1";
    string DeActivated = "0";
    List<DryWhOrder> StoreOrderCheckList = await db.Dry_wh_orders.GroupBy(p => new { p.is_approved_prepa_date, p.total_state_repack_cancelled_qty }).Select(g => g.First()).Where(temp => temp.is_active.Equals(true)
    && temp.is_for_validation.Contains(DeActivated) && temp.is_approved != null && temp.is_prepared != null && temp.is_wh_approved == null && temp.total_state_repack_cancelled_qty != null && temp.is_wh_checker_cancel != null || temp.force_prepared_status != null).ToListAsync();
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

      //string data_is_pending = "1";
      string is_activated = "1";
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
    public async Task<IActionResult> Put([FromBody] DryWhOrder project)
    {
      DryWhOrder existingProject = await db.Dry_wh_orders.Where(temp => temp.is_approved_prepa_date == project.is_approved_prepa_date && temp.store_name == project.store_name && temp.route == project.route && temp.area == project.area && temp.category == project.category ).FirstOrDefaultAsync();
      if (existingProject != null)
      {
        existingProject.is_wh_approved = project.is_wh_approved;
        existingProject.is_wh_approved_by = project.is_wh_approved_by;
        existingProject.is_wh_approved_date = project.is_wh_approved_date;
        existingProject.wh_checker_move_order_no = project.wh_checker_move_order_no;
       
      

       await db.SaveChangesAsync();

        DryWhOrder existingProject2 = await db.Dry_wh_orders.Where(temp => temp.is_approved_prepa_date == project.is_approved_prepa_date && temp.store_name == project.store_name && temp.route == project.route && temp.area == project.area && temp.category == project.category).FirstOrDefaultAsync();
        DryWhOrderViewModel projectViewModel = new DryWhOrderViewModel()
        {
          Is_wh_approved= existingProject2.is_wh_approved,
          Is_wh_approved_by = existingProject2.is_wh_approved_by,
          Is_wh_approved_date = existingProject2.is_wh_approved_date,
          Wh_checker_move_order_no = existingProject2.wh_checker_move_order_no
         


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
      DryWhOrder existingProject = await db.Dry_wh_orders.Where(temp => temp.is_approved_prepa_date == project.is_approved_prepa_date && temp.primary_id == project.primary_id).FirstOrDefaultAsync();
      if (existingProject != null)
      {
        existingProject.is_wh_checker_cancel = project.is_wh_checker_cancel;
        existingProject.is_wh_checker_cancel_by = project.is_wh_checker_cancel_by;
        existingProject.is_wh_checker_cancel_date = project.is_wh_checker_cancel_date;
        existingProject.is_wh_checker_cancel_reason = project.is_wh_checker_cancel_reason;

        await db.SaveChangesAsync();

        DryWhOrder existingProject2 = await db.Dry_wh_orders.Where(temp => temp.is_approved_prepa_date == project.is_approved_prepa_date && temp.primary_id == project.primary_id).FirstOrDefaultAsync();
        DryWhOrderViewModel projectViewModel = new DryWhOrderViewModel()
        {
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
    public async Task<IActionResult> PutCancelPreparedCancelledCountItem([FromBody] DryWhOrder project)
    {
      DryWhOrder existingProject = await db.Dry_wh_orders.Where(temp => temp.is_approved_prepa_date == project.is_approved_prepa_date && temp.primary_id == project.primary_id).FirstOrDefaultAsync();
      if (existingProject != null)
      {
        existingProject.is_wh_checker_cancel = project.is_wh_checker_cancel;
        existingProject.is_wh_checker_cancel_by = project.is_wh_checker_cancel_by;
        existingProject.is_wh_checker_cancel_date = project.is_wh_checker_cancel_date;
        existingProject.is_wh_checker_cancel_reason = project.is_wh_checker_cancel_reason;
        existingProject.total_state_repack_cancelled_qty = project.total_state_repack_cancelled_qty;


        await db.SaveChangesAsync();

        DryWhOrder existingProject2 = await db.Dry_wh_orders.Where(temp => temp.is_approved_prepa_date == project.is_approved_prepa_date && temp.primary_id == project.primary_id).FirstOrDefaultAsync();
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
