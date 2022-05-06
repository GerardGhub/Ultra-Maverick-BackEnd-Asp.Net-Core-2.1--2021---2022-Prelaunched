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
          Mrs_order_by = material.mrs_order_by,
          Mrs_order_date = material.mrs_order_date,
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
