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
  public class ChildCheckListController : Controller
  {

    private ApplicationDbContext db;
    public ChildCheckListController(ApplicationDbContext db)
    {
      this.db = db;
    }


    [HttpPost]
    [Route("api/child_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Post([FromBody] ChildCheckList ChildRequestParam)
    {

      if (ChildRequestParam.cc_parent_key == 0 || ChildRequestParam.cc_parent_key == 0)
      {
        return BadRequest(new { message = "Fill up the required fields" });
      }

      // Start of Getting the Parent Description Key
      var ParentKeyGetandSet = await db.Parent_checklist.Where(temp => temp.parent_chck_id == Convert.ToInt32(ChildRequestParam.cc_parent_key)
    ).ToListAsync();


      string ParentKeyDescription = "";
      int ParentPrimaryKey = 0;
      foreach (var form in ParentKeyGetandSet)
      {
        ParentKeyDescription = form.parent_chck_details;
        ParentPrimaryKey = form.parent_chck_id;
      }

      ChildRequestParam.parent_chck_details = ParentKeyDescription;
      ChildRequestParam.parent_chck_id = ParentPrimaryKey;
      //End of getting the Child Key



      var CheckParentForeignKey = await db.Parent_checklist.Where(temp => temp.parent_chck_id == ChildRequestParam.cc_parent_key
      ).ToListAsync();

      if (CheckParentForeignKey.Count > 0)
      {

      }
      else
      {
      return BadRequest(new { message = "Parent key(" + ChildRequestParam.cc_parent_key + ") is not registered on the system" });
      }


      var ChildCheckListDataInfo = await db.Child_checklist.Where(temp => temp.cc_description == ChildRequestParam.cc_description
      ).ToListAsync();

      if (ChildCheckListDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }


      db.Child_checklist.Add(ChildRequestParam);
      await db.SaveChangesAsync();

      ChildCheckList existingProject = await db.Child_checklist.Where(temp => temp.cc_id == ChildRequestParam.cc_id).FirstOrDefaultAsync();

      ChildCheckListViewModel ChildViewModel = new ChildCheckListViewModel()
      {

        Cc_id = existingProject.cc_id,
        Cc_description = existingProject.cc_description,
        Cc_parent_key = existingProject.cc_parent_key.ToString(),
        Cc_bool_status = existingProject.cc_bool_status,
        Is_active = existingProject.is_active,
        Cc_added_by = existingProject.cc_added_by,
        Cc_date_added = existingProject.cc_date_added,
        Parent_chck_id = ParentPrimaryKey.ToString(),
        Parent_chck_details = ParentKeyDescription
      };

      return Ok(ChildViewModel);

    }


    [HttpPut]
    [Route("api/child_checklist/deactivate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ChildCheckList>> PutDeactivate([FromBody] ChildCheckList ChildRequestParam)
    {



      ChildCheckList existingDataStatus = await db.Child_checklist.Where(temp => temp.cc_id == ChildRequestParam.cc_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.is_active = false;
        existingDataStatus.deactivated_at = DateTime.Now.ToString();
        existingDataStatus.deactivated_by = ChildRequestParam.deactivated_by;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }

    [HttpPut]
    [Route("api/child_checklist/activate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ChildCheckList>> PutActivate([FromBody] ChildCheckList ChildRequestParam)
    {



      ChildCheckList existingDataStatus = await db.Child_checklist.Where(temp => temp.cc_id == ChildRequestParam.cc_id).FirstOrDefaultAsync();
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


    [HttpPut]
    [Route("api/child_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ChildCheckList>> Put([FromBody] ChildCheckList ChildRequestParam)
    {


      var CheckParentForeignKey = await db.Parent_checklist.Where(temp => temp.parent_chck_id == ChildRequestParam.cc_parent_key
      ).ToListAsync();

      if (CheckParentForeignKey.Count > 0)
      {

      }
      else
      {
        return BadRequest(new { message = "Parent key(" + ChildRequestParam.cc_parent_key + ") is not registered on the system" });
      }


      var ChildCheckListDataInfo = await db.Child_checklist
        .Where(temp => temp.cc_description == ChildRequestParam.cc_description).ToListAsync();

      if (ChildCheckListDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }

      ChildCheckList existingDataStatus = await db.Child_checklist.Where(temp => temp.cc_id == ChildRequestParam.cc_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.cc_description = ChildRequestParam.cc_description;
        existingDataStatus.updated_at = DateTime.Now.ToString();
        existingDataStatus.updated_by = ChildRequestParam.updated_by;
        existingDataStatus.cc_parent_key = ChildRequestParam.cc_parent_key;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }



    [HttpGet]
    [Route("api/child_checklist/search/{parent_fk}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetChildCheckList(int parent_fk)
    {

      List<ChildCheckList> allChildCheckList = await db.Child_checklist.Where(temp => temp.cc_parent_key == parent_fk).ToListAsync();


      List<ChildCheckListViewModel> ListViewModel = new List<ChildCheckListViewModel>();

      if (allChildCheckList.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var form in allChildCheckList)
      {

        ListViewModel.Add(new ChildCheckListViewModel()
        {
          Cc_description = form.cc_description,
          Cc_id = form.cc_id,
          Cc_parent_key = form.cc_parent_key.ToString(),
          Cc_bool_status = form.cc_bool_status,
          Cc_added_by = form.cc_added_by,
          Cc_date_added = form.cc_date_added,
          Updated_at = form.updated_at,
          Updated_by = form.updated_by,
          Deactivated_at = form.deactivated_at,
          Deactivated_by = form.deactivated_by,
          Is_active = form.is_active,
          Parent_chck_details = form.parent_chck_details


        });
      }
      return Ok(ListViewModel);


    }





    [HttpGet]
    [Route("api/child_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetChildCheckList()
    {

      List<ChildCheckList> allChildCheckList = await db.Child_checklist.Where(temp => temp.is_active.Equals(true)).ToListAsync();


      List<ChildCheckListViewModel> ListViewModel = new List<ChildCheckListViewModel>();

      if (allChildCheckList.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var form in allChildCheckList)
      {

         ListViewModel.Add(new ChildCheckListViewModel()
        {
          Cc_description = form.cc_description,
          Cc_id = form.cc_id,
          Cc_parent_key = form.cc_parent_key.ToString(),
          Cc_bool_status = form.cc_bool_status,
          Cc_added_by = form.cc_added_by,
          Cc_date_added = form.cc_date_added,
          Updated_at = form.updated_at,
          Updated_by = form.updated_by,
          Deactivated_at = form.deactivated_at,
          Deactivated_by = form.deactivated_by,
          Is_active = form.is_active,
          Parent_chck_details = form.parent_chck_details


         });
      }
      return Ok(ListViewModel);


    }










  }
}
