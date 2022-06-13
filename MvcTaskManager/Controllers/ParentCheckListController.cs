using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Controllers
{
  public class ParentCheckListController : Controller
  {

    private ApplicationDbContext db;
    public ParentCheckListController(ApplicationDbContext db)
    {
      this.db = db;
    }







    [HttpPut]
    [Route("api/parent_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ParentCheckList>> Put([FromBody] ParentCheckList parentRequestParam)
    {

      var ParentCheckListDataInfo = await db.Parent_checklist
        .Where(temp => temp.parent_chck_details == parentRequestParam.parent_chck_details).ToListAsync();

      if (ParentCheckListDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }

      ParentCheckList existingDataStatus = await db.Parent_checklist.Where(temp => temp.parent_chck_id == parentRequestParam.parent_chck_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.parent_chck_details = parentRequestParam.parent_chck_details;
        existingDataStatus.updated_at = DateTime.Now.ToString();
        existingDataStatus.updated_by = parentRequestParam.updated_by;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }

    [HttpPut]
    [Route("api/parent_checklist/deactivate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ParentCheckList>> PutDeactivate([FromBody] ParentCheckList parentRequestParam)
    {
      ParentCheckList existingDataStatus = await db.Parent_checklist.Where(temp => temp.parent_chck_id == parentRequestParam.parent_chck_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.is_active = false;
        existingDataStatus.deactivated_at = DateTime.Now.ToString();
        existingDataStatus.deactivated_by = parentRequestParam.deactivated_by;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }


    [HttpPut]
    [Route("api/parent_checklist/activate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ParentCheckList>> PutActivate([FromBody] ParentCheckList parentRequestParam)
    {

      ParentCheckList existingDataStatus = await db.Parent_checklist.Where(temp => temp.parent_chck_id == parentRequestParam.parent_chck_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.is_active = true;
        existingDataStatus.deactivated_at = null;
        existingDataStatus.deactivated_by = null;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }



    [HttpPost]
    [Route("api/parent_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Post([FromBody] ParentCheckList parentRequestParam)
    {

      if (parentRequestParam.parent_chck_details == null || parentRequestParam.parent_chck_details == ""
        || parentRequestParam.parent_chck_added_by == null || parentRequestParam.parent_chck_added_by == "")
      {
        return BadRequest(new { message = "Fill up the required fields" });
      }

      var ParentCheckListDataInfo = await db.Parent_checklist.Where(temp => temp.parent_chck_details == parentRequestParam.parent_chck_details
      ).ToListAsync();

      if (ParentCheckListDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }


      db.Parent_checklist.Add(parentRequestParam);
      await db.SaveChangesAsync();

      ParentCheckList existingProject = await db.Parent_checklist.Where(temp => temp.parent_chck_id == parentRequestParam.parent_chck_id).FirstOrDefaultAsync();

      ParentCheckListViewModel ParentViewModel = new ParentCheckListViewModel()
      {

        Parent_chck_id = existingProject.parent_chck_id,
        Parent_chck_details = existingProject.parent_chck_details,
        Parent_chck_added_by = existingProject.parent_chck_added_by,
        Parent_chck_date_added = existingProject.parent_chck_date_added,
        Is_active = existingProject.is_active
      };

      return Ok(ParentViewModel);

    }




    [HttpGet]
    [Route("api/parent_checklist/{parent_identity}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> GetParentCheckListById(int parent_identity)
    {

      List<ParentCheckList> AllParentData = await db.Parent_checklist.Where(temp => temp.is_active.Equals(true) && temp.parent_chck_id == parent_identity).ToListAsync();


      List<ParentCheckListViewModel> MaterialRequestViewModel = new List<ParentCheckListViewModel>();

      if (AllParentData.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var material in AllParentData)
      {

        MaterialRequestViewModel.Add(new ParentCheckListViewModel()
        {
          Parent_chck_id = material.parent_chck_id,
          Parent_chck_details = material.parent_chck_details,
          Parent_chck_added_by = material.parent_chck_added_by,
          Parent_chck_date_added = material.parent_chck_date_added,
          Is_active = material.is_active


        });
      }
      return Ok(MaterialRequestViewModel);


    }




    [HttpGet]
    [Route("api/parent_dynamic_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    //public async Task<IActionResult> GetDynamicChecklist()
    //{

    //  List<ParentCheckList> AllParentData = await db.Parent_checklist.Where(temp => temp.is_active.Equals(true)).ToListAsync();


    //  List<ParentCheckListViewModel> MaterialRequestViewModel = new List<ParentCheckListViewModel>();

    //  if (AllParentData.Count > 0)
    //  {

    //  }
    //  else
    //  {
    //    return NoContent();
    //  }

    //  foreach (var material in AllParentData)
    //  {

    //    MaterialRequestViewModel.Add(new ParentCheckListViewModel()
    //    {
    //      Parent_chck_id = material.parent_chck_id,
    //      Parent_chck_details = material.parent_chck_details,
    //      Parent_chck_added_by = material.parent_chck_added_by,
    //      Parent_chck_date_added = material.parent_chck_date_added,
    //      Is_active = material.is_active,
    //      Books = BadImageFormatException.


    //    });
    //  }



    //  return Ok(MaterialRequestViewModel);


    //}

    public async Task<ActionResult<ParentCheckList>> GetDynamicChecklist()
    {

      var DynamicCheckList = await db.Parent_checklist
        .Include(pub => pub.ChildCheckLists)
        .Include(pub => pub.GrandChildCheckLists)
        .Where(pub => pub.is_active.Equals(true))
        .ToListAsync();

      if (DynamicCheckList == null)
      {
        return NotFound();
      }
      return Ok(DynamicCheckList);

    }







    [HttpGet]
    [Route("api/parent_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> GetParentCheckList()
    {

      List<ParentCheckList> AllParentData = await db.Parent_checklist.Where(temp => temp.is_active.Equals(true)).ToListAsync();


      List<ParentCheckListViewModel> MaterialRequestViewModel = new List<ParentCheckListViewModel>();

      if (AllParentData.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var material in AllParentData)
      {

        MaterialRequestViewModel.Add(new ParentCheckListViewModel()
        {
          Parent_chck_id = material.parent_chck_id,
          Parent_chck_details = material.parent_chck_details,
          Parent_chck_added_by = material.parent_chck_added_by,
          Parent_chck_date_added = material.parent_chck_date_added,         
          Is_active = material.is_active


        });
      }
      return Ok(MaterialRequestViewModel);


    }
  }








  ///
}

