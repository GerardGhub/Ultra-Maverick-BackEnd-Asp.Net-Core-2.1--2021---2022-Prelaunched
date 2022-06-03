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

      List<MaterialRequestMaster> allmrs = await db.Material_request_master.Where(temp => temp.is_active.Equals(true)).ToListAsync();
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
          Is_active = material.is_active,
          Is_approved_by = material.is_approved_by,
          Is_approved_date = material.is_approved_date,
          User_id = material.user_id


        });
      }
      return Ok(MaterialRequestViewModel);


    }



    [HttpGet]
    [Route("api/material_request_master/cancel")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> GetCancelMRS()
    {

      List<MaterialRequestMaster> allmrs = await db.Material_request_master.Where(temp => temp.is_active.Equals(false)).ToListAsync();
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
          Is_active = material.is_active,
          Is_approved_by = material.is_approved_by,
          Is_approved_date = material.is_approved_date


        });
      }
      return Ok(MaterialRequestViewModel);


    }



    [HttpPut]
    [Route("api/material_request_master/deactivate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestMaster> PutCancelAll([FromBody] MaterialRequestMaster MRSParams)
    {
      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
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
    [Route("api/material_request_master")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<MaterialRequestMaster>> PutUpdateAll([FromBody] MaterialRequestMaster MRSParams)
    {

      var CheckParametersKey =
        await db.Material_request_master.Where(temp => temp.mrs_req_desc.ToString() == MRSParams.mrs_req_desc).ToListAsync();

      if (CheckParametersKey.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }
 



      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

          item.mrs_req_desc = MRSParams.mrs_req_desc;
          item.updated_by = MRSParams.updated_by;
          item.updated_date = DateTime.Now.ToString("M/d/yyyy");

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
    [Route("api/material_request_master/approve")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestMaster> PutforApprove([FromBody] MaterialRequestMaster MRSParams)
    {
      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

          item.is_approved_by = MRSParams.is_approved_by;
          item.is_approved_date = DateTime.Now.ToString("M/d/yyyy");

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
    [Route("api/material_request_master/dis-approve")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MaterialRequestMaster> PutDisApprove([FromBody] MaterialRequestMaster MRSParams)
    {
      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
      if (existingDataStatus != null)
      {
        foreach (var item in allToBeUpdated)
        {

     
          item.is_cancel_by = MRSParams.is_cancel_by;
          item.is_cancel_date = DateTime.Now.ToString("M/d/yyyy");
          item.is_cancel_reason = MRSParams.is_cancel_reason;
          item.is_approved_by = null;
          item.is_approved_date = null;
          item.is_active = false;

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
      MaterialRequestMaster existingDataStatus = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).FirstOrDefaultAsync();

      var allToBeUpdated = await db.Material_request_master.Where(temp => temp.mrs_id == MRSParams.mrs_id).ToListAsync();
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



      var RawDataInfo = await db.Material_request_master.Where(temp => temp.mrs_requested_date == materialRequest.mrs_requested_date
      && temp.mrs_req_desc == materialRequest.mrs_req_desc && materialRequest.is_active.Equals(true)).ToListAsync();

      if (RawDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }
   

        db.Material_request_master.Add(materialRequest);
        await db.SaveChangesAsync();
      
        MaterialRequestMaster existingProject = await db.Material_request_master.Where(temp => temp.mrs_id == materialRequest.mrs_id).FirstOrDefaultAsync();

      


      MaterialRequestMasterViewModel MRISViewModel = new MaterialRequestMasterViewModel()
      {

        Mrs_id = existingProject.mrs_id,
        Mrs_req_desc = existingProject.mrs_req_desc,
        Mrs_requested_by = existingProject.mrs_requested_by,
        Mrs_requested_date = DateTime.Now.ToString(),
        Department_id = existingProject.department_id,
        Is_active = true,
        User_id = existingProject.user_id


      };

      return Ok(MRISViewModel);

    }




  }





}
