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
  public class MaterialRequestMasterController : Controller
  {
    private ApplicationDbContext db;
    public MaterialRequestMasterController(ApplicationDbContext db)
    {
      this.db = db;
    }




    [HttpGet]
    [Route("api/material_request_master")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Get()
    {

      List<MaterialRequestMaster> allmrs = await db.material_request_master.Where(temp => temp.is_active.Equals(true)).ToListAsync();
      List<MaterialRequestMasterViewModel> MaterialRequestViewModel = new List<MaterialRequestMasterViewModel>();
      foreach (var material in allmrs)
      {

        MaterialRequestViewModel.Add(new MaterialRequestMasterViewModel()
        {
          Mrs_id = material.mrs_id,
          Mrs_req_desc = material.mrs_req_desc,
          Mrs_requested_by = material.mrs_requested_by,
          Mrs_requested_date = material.mrs_requested_date,
          Department_id = material.department_id,
         Is_cancel_by = material.is_cancel_by,
          Is_cancel_reason = material.is_cancel_reason,
          Is_cancel_date = material.is_cancel_date,
          Is_active = material.is_active


        });
      }
      return Ok(MaterialRequestViewModel);


    }




    [HttpPut]
    [Route("api/material_request_master/deactivate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestMaster> PutCancelAll([FromBody] MaterialRequestMaster MRSParams)
    {
      MaterialRequestMaster existingDataStatus = await db.material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

          item.is_active = false;
          item.is_cancel_reason = MRSParams.is_cancel_reason;
          item.is_cancel_by = MRSParams.is_cancel_by;
          item.is_cancel_date = DateTime.Now.ToString("M/d/yyyy");

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
    [Route("api/material_request_master/activate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestMaster> PutReturnAll([FromBody] MaterialRequestMaster MRSParams)
    {
      MaterialRequestMaster existingDataStatus = await db.material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

          item.is_active = true;
          item.is_cancel_reason = null;
          item.is_cancel_by = null;
          item.is_cancel_date = null;

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
    [Route("api/material_request_master")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Post([FromBody] MaterialRequestMaster materialRequest)
    {



      var RawDataInfo = await db.material_request_master.Where(temp => temp.mrs_requested_date == materialRequest.mrs_requested_date
      && temp.mrs_req_desc == materialRequest.mrs_req_desc && materialRequest.is_active.Equals(true)).ToListAsync();

      if (RawDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }
   

        db.material_request_master.Add(materialRequest);
        await db.SaveChangesAsync();
      
        MaterialRequestMaster existingProject = await db.material_request_master.Where(temp => temp.mrs_id == materialRequest.mrs_id).FirstOrDefaultAsync();

      


      MaterialRequestMasterViewModel MRISViewModel = new MaterialRequestMasterViewModel()
      {

        Mrs_id = existingProject.mrs_id,
        Mrs_req_desc = existingProject.mrs_req_desc,
        Mrs_requested_by = existingProject.mrs_requested_by,
        Mrs_requested_date = DateTime.Now.ToString(),
        Department_id = existingProject.department_id,
        Is_active = true


      };

      return Ok(MRISViewModel);

    }




  }





}
