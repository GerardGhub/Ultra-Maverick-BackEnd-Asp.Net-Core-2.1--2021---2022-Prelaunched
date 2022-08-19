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
  public class CheckListParametersController : Controller
  {
    private ApplicationDbContext db;
    public CheckListParametersController(ApplicationDbContext db)
    {
      this.db = db;
    }


    [HttpPost]
    [Route("api/checklist_paramaters")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Post([FromBody] CheckListParameters RequestParam)
    {

    
      if (RequestParam.Cp_gchild_key == null || RequestParam.Cp_gchild_key == "")
      {
        return BadRequest(new { message = "Fill up the required fields" });
      }


      var CheckChildForeignKey = await db.Grandchild_checklist.Where(temp => temp.gc_id.ToString() == RequestParam.Cp_gchild_key
      ).ToListAsync();

      if (CheckChildForeignKey.Count > 0)
      {

      }
      else
      {
        return BadRequest(new { message = "Grand Child key(" + RequestParam.Cp_gchild_key + ") is not registered on the system" });
      }


      var CheckListParamsDataInfo = await db.Checklist_paramaters
        .Where(temp => temp.Cp_description == RequestParam.Cp_description
        && temp.Cp_gchild_key == RequestParam.Cp_gchild_key).ToListAsync();

      if (RequestParam.Cp_description == "yes")
      {

      }
      else if (RequestParam.Cp_description == "manual")
      {

      }


      else
      {

        if (CheckListParamsDataInfo.Count > 0)
        {
          return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
        }
     

      }







      //  var ChildParentKeyGetandSet = await db.Child_checklist.Where(temp => temp.cc_id == Convert.ToInt32(RequestParam.Cp_gchild_key)
      //).ToListAsync();


      string ParentKeyDescription = "";

      //  foreach (var form in ChildParentKeyGetandSet)
      //  {
      //  ParentKeyDescription = form.parent_chck_details;

      //  }


      var GetGranchildDescription = await db.Grandchild_checklist.Where(temp => temp.gc_id == Convert.ToInt32(RequestParam.Cp_gchild_key)
      ).ToListAsync();


      string GCDescription = "";
      int ParentPrimaryKey = 0;
      foreach (var form in GetGranchildDescription)
      {

        GCDescription = form.gc_description;
        ParentPrimaryKey = form.parent_chck_id;
        ParentKeyDescription = form.parent_chck_details;
      }

      RequestParam.Gc_description = GCDescription;
      RequestParam.Parent_chck_details = ParentKeyDescription;
      RequestParam.Parent_chck_id = ParentPrimaryKey;
      RequestParam.Parent_chck_id_fk = ParentPrimaryKey;
      RequestParam.Gc_id = Convert.ToInt32(RequestParam.Cp_gchild_key);
      //End of getting the Child Key


      db.Checklist_paramaters.Add(RequestParam);

      await db.SaveChangesAsync();

      CheckListParameters existingProject = await db.Checklist_paramaters.Where(temp => temp.Cp_params_id == RequestParam.Cp_params_id).FirstOrDefaultAsync();

      CheckListParametersViewModel ChildViewModel = new CheckListParametersViewModel()
      {

        Cp_params_id = existingProject.Cp_params_id,
        Cp_description = existingProject.Cp_description,
        Cp_gchild_key = existingProject.Cp_gchild_key,
        Is_active = existingProject.Is_active,
        Cp_added_by = existingProject.Cp_added_by,
        Cp_date_added = existingProject.Cp_date_added,
        Parent_chck_id_fk = ParentPrimaryKey,
        Parent_chck_id = ParentPrimaryKey,
        Gc_id = Convert.ToInt32(existingProject.Cp_gchild_key),
        Parent_chck_details = ParentKeyDescription
      };

      return Ok(ChildViewModel);

    }


    [HttpPut]
    [Route("api/checklist_paramaters")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<CheckListParameters>> Put([FromBody] CheckListParameters RequestParam)
    {

      //var DataInfo = await db.Checklist_paramaters
      //  .Where(temp => temp.cp_description == RequestParam.cp_description).ToListAsync();

      //if (DataInfo.Count > 0)
      //{
      //  return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      //}

      CheckListParameters existingDataStatus = await db.Checklist_paramaters.Where(temp => temp.Cp_params_id == RequestParam.Cp_params_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Cp_description = RequestParam.Cp_description;
        existingDataStatus.Updated_at = DateTime.Now.ToString();
        existingDataStatus.Updated_by = RequestParam.Updated_by;
        //existingDataStatus.parent_chck_id_fk = RequestParam.parent_chck_id_fk;
        //existingDataStatus.parent_chck_id = RequestParam.parent_chck_id_fk;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }


    [HttpPut]
    [Route("api/checklist_paramaters/deactivate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<CheckListParameters>> PutDeactivate([FromBody] CheckListParameters RequestParam)
    {


      CheckListParameters existingDataStatus = await db.Checklist_paramaters
        .Where(temp => temp.Cp_params_id == RequestParam.Cp_params_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Is_active = false;
        existingDataStatus.Deactivated_at = DateTime.Now.ToString();
        existingDataStatus.Deactivated_by = RequestParam.Deactivated_by;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }


    [HttpPut]
    [Route("api/checklist_paramaters/activate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<CheckListParameters>> PutActivate([FromBody] CheckListParameters RequestParam)
    {
      CheckListParameters existingDataStatus = await db.Checklist_paramaters
        .Where(temp => temp.Cp_params_id == RequestParam.Cp_params_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Is_active = true;
        existingDataStatus.Deactivated_at = null;
        existingDataStatus.Deactivated_by = null;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }



    [HttpGet]
    [Route("api/checklist_paramaters/search/{parent_fk}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ParameterCheckList(string parent_fk)
    {

      List<CheckListParameters> allParamList = await db.Checklist_paramaters.Where(temp => temp.Is_active.Equals(true) && temp.Cp_gchild_key.Contains(parent_fk)).ToListAsync();


      List<CheckListParametersViewModel> ListViewModel = new List<CheckListParametersViewModel>();

      if (allParamList.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var form in allParamList)
      {

        ListViewModel.Add(new CheckListParametersViewModel()
        {
          Cp_description = form.Cp_description,
          Cp_params_id = form.Cp_params_id,
          Cp_gchild_key = form.Cp_gchild_key,
          Parent_chck_details = form.Parent_chck_details,
          Cp_added_by = form.Cp_added_by,
          Cp_date_added = form.Cp_date_added,
          Updated_at = form.Updated_at,
          Updated_by = form.Updated_by,
          Deactivated_at = form.Deactivated_at,
          Deactivated_by = form.Deactivated_by,
          Is_active = form.Is_active,
          Parent_chck_id_fk = form.Parent_chck_id_fk


        });
      }
      return Ok(ListViewModel);


    }





    [HttpGet]
    [Route("api/checklist_paramaters")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetParametersCheckList()
    {

      List<CheckListParameters> allParametersCheckList =
        await db.Checklist_paramaters.ToListAsync();
      //await db.Checklist_paramaters.Where(temp => temp.is_active.Equals(true)).ToListAsync();
      List<CheckListParametersViewModel> ListViewModel = new List<CheckListParametersViewModel>();
      if (allParametersCheckList.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var form in allParametersCheckList)
      {

        ListViewModel.Add(new CheckListParametersViewModel()
        {
          Gc_id = form.Gc_id,
          Gc_description = form.Gc_description,
          Cp_params_id = form.Cp_params_id,
          Cp_description = form.Cp_description,
          Parent_chck_id_fk = form.Parent_chck_id_fk,
          Parent_chck_details = form.Parent_chck_details,
          Cp_added_by = form.Cp_added_by,
          Cp_date_added = form.Cp_date_added,
          Updated_at = form.Updated_at,
          Updated_by = form.Updated_by,
          Deactivated_at = form.Deactivated_at,
          Deactivated_by = form.Deactivated_by,
          Is_active = form.Is_active,
          Cp_gchild_key = form.Cp_gchild_key,


        });
      }
      return Ok(ListViewModel);


    }









  }
}
