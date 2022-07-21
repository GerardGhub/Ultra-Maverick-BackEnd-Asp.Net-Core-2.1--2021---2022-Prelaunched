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
    public async Task<ActionResult<DryWhOrder>> GetDistinctPreparedOrders()
    {
      //string Activated = "1";
      //string DeActivated = "0";
      //List<DryWhOrder> StoreOrderCheckList = await db.Dry_wh_orders.GroupBy(p => new {p.is_approved_prepa_date, p.fox}).Select(g => g.First()).Where(temp => temp.is_active.Contains(Activated)
      //&& temp.is_for_validation.Contains(DeActivated)
      //&& temp.is_approved != null
      //&& temp.is_prepared != null
      //&& temp.is_wh_approved == null
      //|| temp.force_prepared_status != null).ToListAsync();
      //return StoreOrderCheckList;

      var result = await (from DryWhOrder in db.Dry_wh_orders
                            //join User in db.Users on Parents.user_id equals User.User_Identity
                            //join Department in db.Department on Parents.department_id equals Department.department_id

                          where DryWhOrder.primary_id == DryWhOrder.primary_id
                            //&& Parents.is_approved_by == null
                            && DryWhOrder.is_active.Equals(true)



                          select new
                          {
                            DryWhOrder.primary_id,
                            DryWhOrder.is_approved_prepa_date,
                            DryWhOrder.store_name
                            //,

                            //total_request_count = (from DryWhOrder in db.Material_request_logs
                            //                       where Parents.mrs_id == Childs.mrs_id
                            //                       && Childs.is_active.Equals(true)
                            //                       select Parents).Count(),




                          })

               .ToListAsync();

      return Ok(result);



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
    public async Task<List<DryWhOrder>> GetStoreOrders()
    {
        string Activated = "1";
        string DeActivated = "0";
        List<DryWhOrder> StoreOrderCheckList = await db.Dry_wh_orders.GroupBy(p => new { p.is_approved_prepa_date }).Select(g => g.First()).Where(temp => temp.is_active.Equals(true)
        && temp.is_for_validation.Contains(DeActivated)
        && temp.is_approved != null
        && temp.is_prepared == null
        || temp.force_prepared_status != null).ToListAsync();
        return StoreOrderCheckList;
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
