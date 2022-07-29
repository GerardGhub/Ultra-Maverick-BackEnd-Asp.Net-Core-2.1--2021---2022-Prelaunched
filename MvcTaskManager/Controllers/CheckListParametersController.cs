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

    
      if (RequestParam.cp_gchild_key == null || RequestParam.cp_gchild_key == "")
      {
        return BadRequest(new { message = "Fill up the required fields" });
      }


      var CheckChildForeignKey = await db.Grandchild_checklist.Where(temp => temp.gc_id.ToString() == RequestParam.cp_gchild_key
      ).ToListAsync();

      if (CheckChildForeignKey.Count > 0)
      {

      }
      else
      {
        return BadRequest(new { message = "Grand Child key(" + RequestParam.cp_gchild_key + ") is not registered on the system" });
      }


      var CheckListParamsDataInfo = await db.Checklist_paramaters
        .Where(temp => temp.cp_description == RequestParam.cp_description).ToListAsync();

      if (RequestParam.cp_description == "yes")
      {

      }
      else if (RequestParam.cp_description == "manual")
      {

      }
      else
      {

        if (CheckListParamsDataInfo.Count > 0)
        {
          return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
        }
     

      }

      //if (CheckListParamsDataInfo.Count > 0)
      //{

      //  var CheckListParamsDataInfoForYes = await db.Checklist_paramaters
      //    .Where(temp => temp.cp_description == RequestParam.cp_description
      //      ).ToListAsync();

      //  return BadRequest(CheckListParamsDataInfo.Count());

      //  //return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      //}
      //else
      //{
      //  return Ok("Bading");
      //}




      //  // Start of Getting the Child Key
      //  var GChildParentKeyGetandSet = await db.Grandchild_checklist.Where(temp => temp.gc_id == Convert.ToInt32(RequestParam.cp_gchild_key)
      //).ToListAsync();


      var ChildParentKeyGetandSet = await db.Child_checklist.Where(temp => temp.cc_id == Convert.ToInt32(RequestParam.cp_gchild_key)
    ).ToListAsync();


      string ParentKeyDescription = "";

      foreach (var form in ChildParentKeyGetandSet)
      {
      ParentKeyDescription = form.parent_chck_details;
  
      }


      var GetGranchildDescription = await db.Grandchild_checklist.Where(temp => temp.gc_id == Convert.ToInt32(RequestParam.cp_gchild_key)
      ).ToListAsync();


      string GCDescription = "";
      int ParentPrimaryKey = 0;
      foreach (var form in GetGranchildDescription)
      {

        GCDescription = form.gc_description;
        ParentPrimaryKey = form.parent_chck_id;
      }

      RequestParam.gc_description = GCDescription;
      RequestParam.parent_chck_details = ParentKeyDescription;
      RequestParam.parent_chck_id = ParentPrimaryKey;
      RequestParam.parent_chck_id_fk = ParentPrimaryKey;
      RequestParam.gc_id = Convert.ToInt32(RequestParam.cp_gchild_key);
      //End of getting the Child Key


      db.Checklist_paramaters.Add(RequestParam);
      await db.SaveChangesAsync();

      CheckListParameters existingProject = await db.Checklist_paramaters.Where(temp => temp.cp_params_id == RequestParam.cp_params_id).FirstOrDefaultAsync();

      CheckListParametersViewModel ChildViewModel = new CheckListParametersViewModel()
      {

        Cp_params_id = existingProject.cp_params_id,
        Cp_description = existingProject.cp_description,
        Cp_gchild_key = existingProject.cp_gchild_key,
        Is_active = existingProject.is_active,
        Cp_added_by = existingProject.cp_added_by,
        Cp_date_added = existingProject.cp_date_added,
        Parent_chck_id_fk = ParentPrimaryKey,
        Parent_chck_id = ParentPrimaryKey,
        Gc_id = Convert.ToInt32(existingProject.cp_gchild_key),
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

      CheckListParameters existingDataStatus = await db.Checklist_paramaters.Where(temp => temp.cp_params_id == RequestParam.cp_params_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.cp_description = RequestParam.cp_description;
        existingDataStatus.updated_at = DateTime.Now.ToString();
        existingDataStatus.updated_by = RequestParam.updated_by;
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


      CheckListParameters existingDataStatus = await db.Checklist_paramaters.Where(temp => temp.cp_params_id == RequestParam.cp_params_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.is_active = false;
        existingDataStatus.deactivated_at = DateTime.Now.ToString();
        existingDataStatus.deactivated_by = RequestParam.deactivated_by;
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
      CheckListParameters existingDataStatus = await db.Checklist_paramaters.Where(temp => temp.cp_params_id == RequestParam.cp_params_id).FirstOrDefaultAsync();
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



    [HttpGet]
    [Route("api/checklist_paramaters/search/{parent_fk}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ParameterCheckList(string parent_fk)
    {

      List<CheckListParameters> allParamList = await db.Checklist_paramaters.Where(temp => temp.is_active.Equals(true) && temp.cp_gchild_key.Contains(parent_fk)).ToListAsync();


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
          Cp_description = form.cp_description,
          Cp_params_id = form.cp_params_id,
          Cp_gchild_key = form.cp_gchild_key,
          Parent_chck_details = form.parent_chck_details,
          //Cp_bool_status = form.cp_bool_status,
          Cp_added_by = form.cp_added_by,
          Cp_date_added = form.cp_date_added,
          Updated_at = form.updated_at,
          Updated_by = form.updated_by,
          Deactivated_at = form.deactivated_at,
          Deactivated_by = form.deactivated_by,
          Is_active = form.is_active,
          Parent_chck_id_fk = form.parent_chck_id_fk


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
          Gc_id = form.gc_id,
          Gc_description = form.gc_description,
          Cp_params_id = form.cp_params_id,
          Cp_description = form.cp_description,
          Parent_chck_id_fk = form.parent_chck_id_fk,
          Parent_chck_details = form.parent_chck_details,
          Cp_added_by = form.cp_added_by,
          Cp_date_added = form.cp_date_added,
          Updated_at = form.updated_at,
          Updated_by = form.updated_by,
          Deactivated_at = form.deactivated_at,
          Deactivated_by = form.deactivated_by,
          Is_active = form.is_active,
          Cp_gchild_key = form.cp_gchild_key,


        });
      }
      return Ok(ListViewModel);


    }









  }
}
