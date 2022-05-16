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




    [HttpPost]
    [Route("api/material_request_logs_insert")]
    [Authorize]
 
    public async Task<IActionResult> Post([FromBody] MaterialRequestLogs materialRequest)
    {

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
        Mrs_date_requested = DateTime.Now.ToString("M/d/yyyy"),
        Mrs_requested_by = existingProject.mrs_requested_by,
        Is_active = existingProject.is_active.ToString()

      //Mrs_order_by = existingProject.mrs_order_by,
      //Mrs_order_date = DateTime.Now.ToString("M/d/yyyy"),
    };

      return Ok(MRISViewModel);

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
          Is_active = material.is_active.ToString()


        });
        }
        return Ok(MaterialRequestViewModel);


      }
    }






}
