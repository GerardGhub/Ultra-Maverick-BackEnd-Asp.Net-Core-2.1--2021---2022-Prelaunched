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
      MaterialRequestLogs existingDataStatus = await db.material_request_logs.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.mrs_order_qty = MRSParams.mrs_order_qty;
        existingDataStatus.mrs_uom = MRSParams.mrs_uom;
        existingDataStatus.mrs_date_needed = MRSParams.mrs_date_needed;
        //existingDataStatus.mrs_date_requested = DateTime.Now.ToString("M/d/yyyy");
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
      MaterialRequestLogs existingDataStatus = await db.material_request_logs.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.is_active = false;
        existingDataStatus.activated_by = null;
        existingDataStatus.activated_date = null;
        existingDataStatus.deactivated_by = MRSParams.deactivated_by;
        existingDataStatus.deactivated_date = DateTime.Now.ToString("M/d/yyyy");

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
      MaterialRequestLogs existingDataStatus = await db.material_request_logs.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.is_active = true;
        existingDataStatus.deactivated_by = null;
        existingDataStatus.deactivated_date = null;
        existingDataStatus.activated_by = MRSParams.activated_by;
        existingDataStatus.activated_date = DateTime.Now.ToString("M/d/yyyy");

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
    public async Task<IActionResult> Post([FromBody] MaterialRequestLogs materialRequest)
    {

      var RawMaterial = await db.Raw_Materials_Dry.Where(temp => temp.item_code == materialRequest.mrs_item_code
      && temp.is_active.Equals(true)).ToListAsync();

      if (RawMaterial.Count > 0)
      {

      }
      else
      {
        return BadRequest(new { message = "Item is not exist" });
      }

      var PrimaryUnit = await db.Primary_Unit.Where(temp => temp.unit_desc == materialRequest.mrs_uom
     && temp.is_active.Equals(true)).ToListAsync();

      if (PrimaryUnit.Count > 0)
      {

      }
      else
      {
        return BadRequest(new { message = "Primary Unit is not exist" });
      }


      var RawDataInfo = await db.material_request_logs.Where(temp => temp.mrs_transact_no == materialRequest.mrs_transact_no
      && temp.mrs_date_needed == materialRequest.mrs_date_needed
      && temp.mrs_order_qty == materialRequest.mrs_order_qty
      && temp.mrs_item_code == materialRequest.mrs_item_code).ToListAsync();

      if (RawDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already request a same item today" });
      }


      db.material_request_logs.Add(materialRequest);
      await db.SaveChangesAsync();

      MaterialRequestLogs existingProject = await db.material_request_logs.Where(temp => temp.mrs_id == materialRequest.mrs_id).FirstOrDefaultAsync();

      string ActualQuantity = materialRequest.mrs_order_qty.ToString();
      decimal qtyorder;
      bool isDecimal = decimal.TryParse(ActualQuantity.ToString(), out qtyorder);

      if (isDecimal == false)
      {
        return BadRequest(new { message = "Invalid Quantity!" });
        //...
      }
      //else
      //{
      //  return BadRequest(new { message = "string is not allowed!" });
      //}


      MaterialRequestLogsViewModel MRISViewModel = new MaterialRequestLogsViewModel()
      {
        Mrs_transact_no = existingProject.mrs_transact_no,
        Mrs_item_code = existingProject.mrs_item_code,
        Mrs_item_description = existingProject.mrs_item_description,
        Mrs_order_qty = existingProject.mrs_order_qty,
        Mrs_uom = existingProject.mrs_uom,
        Mrs_date_needed = existingProject.mrs_date_needed,
        //Mrs_date_requested = DateTime.Now.ToString("M/d/yyyy"),
        Mrs_requested_by = existingProject.mrs_requested_by,
        Is_active = true,
        Department_Id = existingProject.department_id

      //Mrs_order_by = existingProject.mrs_order_by,
      //Mrs_order_date = DateTime.Now.ToString("M/d/yyyy"),
    };

      return Ok(MRISViewModel);

    }



    [HttpGet]
    [Route("api/material_request_logs_distinct")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetDistinctMaterialRequestByTransactNo()
    {

      //List<MaterialRequestLogs> StoreOrderCheckList = await db.material_request_logs.GroupBy(p => new { p.mrs_transact_no, p.mrs_requested_by })
      //  .Select(g => g.First()).Where(temp => temp.is_active.Equals(true)
      //  ).ToListAsync();

      List<MaterialRequestLogs> obj = new List<MaterialRequestLogs>();
      var results = (from p in db.material_request_logs
                     join b in db.Department on p.department_id equals b.department_id
                     where p.is_active.Equals(true)

                     group p by new
                     {
                       p.mrs_transact_no,
                       p.department_id,
                       p.mrs_date_requested
                     } into total

                     select new MaterialRequestDistinctPerTransactions
                     {
                   
                       Mrs_transact_no = total.Key.mrs_transact_no,
                       Mrs_order_qty = total.Sum(x => Convert.ToInt32(x.mrs_order_qty)),
                       Department_Id = total.Key.department_id,
                       Static_count = total.Count(),
                       Mrs_date_requested = total.Key.mrs_date_requested
                       
                     }



                    );


      return Ok(results);
      //  var teamTotalScores =
      //from player in StoreOrderCheckList
      //group player by player.mrs_transact_no into playerGroup
      //select new
      //{
      //  Team = playerGroup.Key,
      //  TotalScore = playerGroup.Sum(x => x.static_count),
      //};

      //return StoreOrderCheckList;

      //return List<MaterialRequestLogs>;
    }


    [HttpGet]
      [Route("api/material_request_logs")]
      [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

      public async Task<IActionResult> Get()
      {
        //List<tblLocation> tblRejectedStatuses = await db.Location.ToListAsync();
        //return Ok(tblRejectedStatuses);
        List<MaterialRequestLogs> allmrs = await db.material_request_logs.Where(temp => temp.is_active.Equals(true)).ToListAsync();
        List<MaterialRequestLogsViewModel> MaterialRequestViewModel = new List<MaterialRequestLogsViewModel>();
        foreach (var material in allmrs)
        {

        MaterialRequestViewModel.Add(new MaterialRequestLogsViewModel()
          {
          Mrs_id = material.mrs_id,
          Mrs_transact_no = material.mrs_transact_no,
          Mrs_item_code = material.mrs_item_code,
          Mrs_item_description = material.mrs_item_description,
          Mrs_order_qty = material.mrs_order_qty,
          Mrs_uom = material.mrs_uom,
          Mrs_served_qty = material.mrs_served_qty,
          Mrs_remarks = material.mrs_remarks,
          Mrs_date_needed = material.mrs_date_needed,
          Mrs_date_requested = material.mrs_date_requested,
          //Mrs_order_by = material.mrs_order_by,
          //Mrs_order_date = material.mrs_order_date,
          Mrs_approved_by = material.mrs_approved_by,
          Mrs_approved_date =  material.mrs_approved_date,
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
