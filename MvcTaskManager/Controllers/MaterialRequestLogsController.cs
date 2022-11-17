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
  public class MaterialRequestLogsController : Controller
  {

    private ApplicationDbContext db;
    public MaterialRequestLogsController(ApplicationDbContext db)
    {
      this.db = db;
    }

    
    [HttpPut]
    [Route("api/material_request_logs_update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestLogs> Put([FromBody] MaterialRequestLogs MRSParams)
    {
      MaterialRequestLogs existingDataStatus = await db.Material_request_logs.Where(temp => temp.id == MRSParams.id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.mrs_order_qty = MRSParams.mrs_order_qty;
        existingDataStatus.mrs_uom = MRSParams.mrs_uom;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }



    [HttpPut]
    [Route("api/material_request_logs_deactivate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestLogs> PutDeactivate([FromBody] MaterialRequestLogs MRSParams)
    {
   
      MaterialRequestLogs existingDataStatus = await db.Material_request_logs.Where(temp => temp.id == MRSParams.id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.is_active = false;
        existingDataStatus.activated_by = null;
        existingDataStatus.activated_date = null;
        existingDataStatus.deactivated_by = MRSParams.deactivated_by;
        existingDataStatus.deactivated_date = DateTime.Now.ToString("M/d/yyyy");
        existingDataStatus.cancel_reason = MRSParams.cancel_reason;
        existingDataStatus.is_wh_checker_cancel = MRSParams.is_wh_checker_cancel = "1";
        existingDataStatus.is_prepared = false;
        await db.SaveChangesAsync();


        MaterialRequestMaster existingParentData = await db.Material_request_master
          .Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();
        if (existingParentData != null)
        {
          existingParentData.is_prepared = false;
        }
        await db.SaveChangesAsync();

        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }







    [HttpPut]
    [Route("api/material_request_logs_activate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestLogs> Putactivate([FromBody] MaterialRequestLogs MRSParams)
    {
      MaterialRequestLogs existingDataStatus = await db.Material_request_logs.Where(temp => temp.id == MRSParams.id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.is_active = true;
        existingDataStatus.deactivated_by = null;
        existingDataStatus.deactivated_date = null;
        existingDataStatus.activated_by = MRSParams.activated_by;
        existingDataStatus.activated_date = DateTime.Now.ToString("M/d/yyyy");
        existingDataStatus.cancel_reason = MRSParams.cancel_reason;
        existingDataStatus.is_wh_checker_cancel = null;
        existingDataStatus.is_prepared = true;
        await db.SaveChangesAsync();


        MaterialRequestLogs checkIfYouHaveCancelledData = await db.Material_request_logs.Where(temp => temp.mrs_id == MRSParams.mrs_id && temp.is_active.Equals(false)).FirstOrDefaultAsync();
        if (checkIfYouHaveCancelledData != null)
        {

        }
        else
        {
                  MaterialRequestMaster existingParentData = await db.Material_request_master
                  .Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();
                  if (existingParentData != null)
                  {
                  existingParentData.is_prepared = true;
                  }
                  await db.SaveChangesAsync();
        }


        return existingDataStatus;
      }
      else
      {
        return null;
      }





    }



    [HttpDelete]
    [Route("api/material_request_logs/{ID}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<int> Delete(int ID)
    {
      MaterialRequestLogs existingTaskPriority = await db.Material_request_logs.Where(temp => temp.id == ID).FirstOrDefaultAsync();
      if (existingTaskPriority != null)
      {
        db.Material_request_logs.Remove(existingTaskPriority);
        db.SaveChanges();

        return ID;
      }
      else
      {
        return -1;
      }
    }






    [HttpPut]
    [Route("api/material_request_logs_activate_cancelallOrder")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestLogs> PutCancelAll([FromBody] MaterialRequestLogs MRSParams)
    {
      MaterialRequestLogs existingDataStatus = await db.Material_request_logs.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_logs.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

          item.is_active = false;
          item.activated_by = null;
          item.activated_date = null;
          item.deactivated_by = MRSParams.deactivated_by;
          item.deactivated_date = DateTime.Now.ToString("M/d/yyyy");
        
        }

      await db.SaveChangesAsync();
      return existingDataStatus;

      }
      else
      {
        return null;
      }
    }







    [HttpPost]
    [Route("api/material_request_logs_insert")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Post([FromBody] MaterialRequestLogs[] materialRequest)
    {
      int incrementationofId = 0;
      var GetAllCountofMasterData = await db.Material_request_master.Where(temp => temp.is_active.Equals(true)).ToListAsync();

      if (GetAllCountofMasterData.Count > 0)
      {


        int getTheLastNumberofKey = await (from r in db.Material_request_master orderby r.mrs_id select r.mrs_id).MaxAsync();

        //return Ok(getTheLastNumberofKey);

        incrementationofId = getTheLastNumberofKey * 1;
      }
      else
      {
        return BadRequest("No Master Data Found!");
      }
      //return Ok(incrementationofId);

      foreach (MaterialRequestLogs items in materialRequest)
      {

        //1
       // var MrsTransactNo = await db.Material_request_master.Where(temp => temp.mrs_id == items.mrs_id
       //&& temp.is_active.Equals(true)).ToListAsync();

       // if (MrsTransactNo.Count > 0)
       // {
       //   db.Material_request_logs.Add(items);
        
       // }
       // else
       // {
       //   return BadRequest(new { message = "Transaction number is not exist" });
       // }


        //Remove muna conflict 6/20/2022
        //var MrsTransactIsApproved = await db.Material_request_master.Where(temp => temp.mrs_id == items.mrs_transact_no
        //&& temp.is_active.Equals(true) && temp.is_approved_by != null).ToListAsync();

        //if (MrsTransactIsApproved.Count > 0)
        //{
        //  //db.Material_request_logs.Add(items);
        
        //}
        //else
        //{
        //  return BadRequest(new { message = "Transaction number is not yet approved" });
        //}
      


        //2
        var RawMaterial = await db.Raw_Materials_Dry.Where(temp => temp.item_code == items.mrs_item_code
        && temp.is_active.Equals(true)).ToListAsync();

        if (RawMaterial.Count > 0)
        {
          db.Material_request_logs.Add(items);
          //await db.SaveChangesAsync();
        }
        else
        {
        return BadRequest(new { message = "Item is not exist" });
        }

        //var MaterialRequestMaster = await db.Material_request_master.Where(src => src.

        //int id = (from r in tableName orderby r.ID select r.ID).MAX();


        //3
        var PrimaryUnit = await db.Primary_Unit.Where(temp => temp.unit_desc == items.mrs_uom
        && temp.is_active.Equals(true)).ToListAsync();

        if (PrimaryUnit.Count > 0)
        {
          //items.mrs_id = incrementationofId;
          var checkTheIncrementingID = await db.Material_request_master.Where(temp => temp.mrs_id == incrementationofId
          ).FirstOrDefaultAsync();
          //if (checkTheIncrementingID.Count > 0)
          if (checkTheIncrementingID.mrs_id == incrementationofId)
          {
            //if(incrementationofId == )
            items.mrs_id = incrementationofId;
          }
          else
          {
            items.mrs_id = incrementationofId + 1;
          }

            db.Material_request_logs.Add(items);
         
        }
        else
        {
        return BadRequest(new { message = "Primary Unit is not exist" });
        }
        //4
        //var RawDataInfo = await db.material_request_logs.Where(temp => temp.mrs_transact_no == items.mrs_transact_no
        //&& temp.mrs_date_needed == items.mrs_date_needed
        //&& temp.mrs_order_qty == items.mrs_order_qty
        //&& temp.mrs_item_code == items.mrs_item_code).ToListAsync();

        //if (RawDataInfo.Count > 0)
        //{
        //return BadRequest(new { message = "You already request a same item today" });
        //}

        
        //5


        MaterialRequestLogs existingProject = await db.Material_request_logs.Where(temp => temp.id == items.id).FirstOrDefaultAsync();

        string ActualQuantity = items.mrs_order_qty.ToString();
        decimal qtyorder;
        bool isDecimal = decimal.TryParse(ActualQuantity.ToString(), out qtyorder);

        if (isDecimal == false)
        {
          return BadRequest(new { message = "Invalid Quantity!" });
        }


        await db.SaveChangesAsync();


      }



      return Ok(materialRequest);
    }




    [HttpGet]
    [Route("api/material_request_logs_distinct/userlogin/{user_id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetDistinctMaterialRequestByUserLogin(int user_id)
    {
      List<MaterialRequestMaster> obj = new List<MaterialRequestMaster>();
      var results = (from a in db.Material_request_master
                     join b in db.Department on a.department_id equals b.department_id
                     join c in db.Material_request_logs on a.mrs_id equals c.mrs_id
                     where a.is_active.Equals(true) && b.is_active.Equals(true) && c.is_active.Equals(true)
                   
                  && a.user_id == user_id

                     group a by new
                     {
                       a.mrs_id,
                       a.mrs_req_desc,
                       a.department_id,
                       a.mrs_requested_date,
                       a.mrs_requested_by,
                       b.department_name,
                       a.user_id,
                       a.is_prepared
                      



                     } into total

                     select new MaterialRequestDistinctPerTransactions
                     {

                       Mrs_transact_no = total.Key.mrs_id.ToString(),
                       //Mrs_order_qty = total.Sum(x => Convert.ToInt32(x.mrs_order_qty)),
                       Mrs_req_desc = total.Key.mrs_req_desc,
                       Department_Id = total.Key.department_id,
                       Static_count = total.Count(),
                       Mrs_date_requested = total.Key.mrs_requested_date,
                       Department_name = total.Key.department_name,
                       Mrs_requested_by = total.Key.mrs_requested_by,
                       User_id = total.Key.user_id,
                       Is_prepared = total.Key.is_prepared.ToString()

                     }



                    );


      //return Ok(results);
      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(results);
      });

      if(results.Count() > 0)
      {
        return (result);
      }
      else
      {
   
        return NoContent();
      }





    }




    [HttpGet]
    [Route("api/material_request_logs_distinct/userlogin/approver/{user_id}/{approver_id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetDistinctMaterialRequestByUserLoginApprover(int user_id, int approver_id)
    {




      List<MaterialRequestMaster> obj = new List<MaterialRequestMaster>();
      var results = (from a in db.Material_request_master
                     join b in db.Department on a.department_id equals b.department_id
                     join c in db.Material_request_logs on a.mrs_id equals c.mrs_id
                     join d in db.Users on a.user_id equals d.User_Identity   //remove id
                     where a.is_active.Equals(true) && b.is_active.Equals(true) && c.is_active.Equals(true)
                     && a.user_id == user_id
                     || d.First_approver_id == approver_id
                     || d.Second_approver_id == approver_id
                     || d.Third_approver_id == approver_id
                     || d.First_approver_id == approver_id

                     group a by new
                     {
                       a.mrs_id,
                       a.mrs_req_desc,
                       a.department_id,
                       a.mrs_requested_date,
                       a.mrs_requested_by,
                       b.department_name,
                       a.user_id,
                       a.is_prepared



                     } into total

                     select new MaterialRequestDistinctPerTransactions
                     {

                       Mrs_transact_no = total.Key.mrs_id.ToString(),
                       //Mrs_order_qty = total.Sum(x => Convert.ToInt32(x.mrs_order_qty)),
                       Department_Id = total.Key.department_id,
                       Static_count = total.Count(),
                       Mrs_date_requested = total.Key.mrs_requested_date,
                       Department_name = total.Key.department_name,
                       Mrs_requested_by = total.Key.mrs_requested_by,
                       User_id = total.Key.user_id,
                       Is_prepared = total.Key.is_prepared.ToString()

                     }



                    );


      //return Ok(results);
      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(results);
      });

      if (results.Count() > 0)
      {
        return (result);
      }
      else
      {

        return NoContent();
      }





    }

    [HttpGet]
    [Route("api/material_request_logs_distinct")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetDistinctMaterialRequestByTransactNo()
    {


      List<MaterialRequestMaster> obj = new List<MaterialRequestMaster>();
      var results = (from a in db.Material_request_master
                     join b in db.Department on a.department_id equals b.department_id
                     join c in db.Material_request_logs on a.mrs_id equals c.mrs_id
                     where a.is_active.Equals(true) && b.is_active.Equals(true) && c.is_active.Equals(true)

                     group a by new
                     {
                       a.mrs_id,
                       a.mrs_req_desc,
                       a.department_id,
                       a.mrs_requested_date,
                       a.mrs_requested_by,
                       b.department_name,
                       a.is_prepared,
                       a.user_id
                 
               
 
                     } into total

                     select new MaterialRequestDistinctPerTransactions
                     {

                       Mrs_transact_no = total.Key.mrs_id.ToString(),
                       //Mrs_order_qty = total.Sum(x => Convert.ToInt32(x.mrs_order_qty)),
                       Mrs_req_desc = total.Key.mrs_req_desc,
                       Department_Id = total.Key.department_id,
                       Static_count = total.Count(),  
                       Mrs_date_requested = total.Key.mrs_requested_date,
                       Department_name = total.Key.department_name,
                       Mrs_requested_by = total.Key.mrs_requested_by,
                       Is_prepared = total.Key.is_prepared.ToString(),
                       User_id = total.Key.user_id
                       //Is_prepared = total.Sum(x => Convert.ToInt32(total.Key.is_prepared))
                       //Is_prepared = (from x in total where x.is_prepared != 1 select x).Count().ToString()
                       //Is_prepared = total.Sum(x => Convert.ToInt32(x.is_prepared)).ToString()
                     }



                    );


      //return Ok(results);
      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(results);
      });

      return (result);



    }



    [HttpGet]
    [Route("api/material_request_logs_distinct_generate_transactno")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetDistinctTransactionNumber()
    {

      //List<MaterialRequestLogs> StoreOrderCheckList = await db.material_request_logs.GroupBy(p => new { p.mrs_transact_no, p.mrs_requested_by })
      //  .Select(g => g.First()).Where(temp => temp.is_active.Equals(true)
      //  ).ToListAsync();

      //int count = StoreOrderCheckList.Count() + 1;


      //return Ok(count);

      List<MaterialRequestMaster> StoreOrderCheckList =
        await db.Material_request_master.GroupBy(p => new { p.mrs_id })
        .Select(g => g.First()).Where(temp => temp.is_active.Equals(true) || temp.is_active.Equals(false)
        ).ToListAsync();

      int count = StoreOrderCheckList.Count() + 1;

   
      return Ok(count);

    }


    [HttpGet]
    [Route("api/material_request_logs")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Get()
    {
   
      List<MaterialRequestLogs> allmrs = await db.Material_request_logs.Where(temp => temp.is_active.Equals(true)).ToListAsync();
      List<MaterialRequestLogsViewModel> MaterialRequestViewModel = new List<MaterialRequestLogsViewModel>();
      foreach (var material in allmrs)
      {

        MaterialRequestViewModel.Add(new MaterialRequestLogsViewModel()
        {
          Id = material.id,
          Mrs_transact_no = material.mrs_id.ToString(),
          Mrs_item_code = material.mrs_item_code,
          Mrs_item_description = material.mrs_item_description,
          Mrs_order_qty = material.mrs_order_qty,
          Mrs_uom = material.mrs_uom,
          Mrs_served_qty = material.mrs_served_qty,
          Mrs_remarks = material.mrs_remarks,
          Mrs_date_requested = material.mrs_date_requested,
          Mrs_approved_by = material.mrs_approved_by,
          Mrs_approved_date = material.mrs_approved_date,
          Mrs_issued_by = material.mrs_issued_by,
          Mrs_issued_date = material.mrs_issued_by,
          Mrs_requested_by = material.mrs_requested_by,
          Is_active = material.is_active,
          Department_Id = material.department_id


        });
      }
      return Ok(MaterialRequestViewModel);


    }





    [HttpGet]
    [Route("api/material_request_logs/search/partial_inactive/{transaction_number}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> GetMaterialOrderPartialCancelled(string transaction_number)
    {

      string transact_no_passed_by = transaction_number;



      List<MaterialRequestLogs> allmrs = await db.Material_request_logs.Where(temp => temp.is_active.Equals(false) && temp.mrs_id.ToString().Contains(transact_no_passed_by)).ToListAsync();
      List<MaterialRequestLogsViewModel> MaterialRequestViewModel = new List<MaterialRequestLogsViewModel>();

      if (allmrs.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var material in allmrs)
      {

        MaterialRequestViewModel.Add(new MaterialRequestLogsViewModel()
        {
          Id = material.id,
          Mrs_transact_no = material.mrs_id.ToString(),
          Mrs_item_code = material.mrs_item_code,
          Mrs_item_description = material.mrs_item_description,
          Mrs_order_qty = material.mrs_order_qty,
          Mrs_uom = material.mrs_uom,
          Mrs_served_qty = material.mrs_served_qty,
          Mrs_remarks = material.mrs_remarks,
          Mrs_date_requested = material.mrs_date_requested,
          Mrs_approved_by = material.mrs_approved_by,
          Mrs_approved_date = material.mrs_approved_date,
          Mrs_issued_by = material.mrs_issued_by,
          Mrs_issued_date = material.mrs_issued_by,
          Mrs_requested_by = material.mrs_requested_by,
          Is_active = material.is_active


        });
      }
      return Ok(MaterialRequestViewModel);


    }



    [HttpGet]
  [Route("api/material_request_logs/search/{transaction_number}")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

  public async Task<IActionResult> GetMaterialOrder(string transaction_number)
  {

      string transact_no_passed_by = transaction_number;



    List<MaterialRequestLogs> allmrs = await db.Material_request_logs.Where(temp => temp.is_active.Equals(true) && temp.mrs_id.ToString().Contains(transact_no_passed_by)).ToListAsync();
    List<MaterialRequestLogsViewModel> MaterialRequestViewModel = new List<MaterialRequestLogsViewModel>();

      if (allmrs.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var material in allmrs)
    {

      MaterialRequestViewModel.Add(new MaterialRequestLogsViewModel()
      {
        Id = material.id,
        Mrs_transact_no = material.mrs_id.ToString(),
        Mrs_item_code = material.mrs_item_code,
        Mrs_item_description = material.mrs_item_description,
        Mrs_order_qty = material.mrs_order_qty,
        Mrs_uom = material.mrs_uom,
        Mrs_served_qty = material.mrs_served_qty,
        Mrs_remarks = material.mrs_remarks,
        Mrs_date_requested = material.mrs_date_requested,
        Mrs_approved_by = material.mrs_approved_by,
        Mrs_approved_date = material.mrs_approved_date,
        Mrs_issued_by = material.mrs_issued_by,
        Mrs_issued_date = material.mrs_issued_by,
        Mrs_requested_by = material.mrs_requested_by,
        Is_active = material.is_active


      });
    }
    return Ok(MaterialRequestViewModel);

     
  }
}





}
