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
  public class GrandChildCheckListController : Controller
  {

    private ApplicationDbContext db;
    public GrandChildCheckListController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/grandchild_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetGrandChildCheckList ()
    {

      List<GrandChildCheckList> allGrandChildCheckList = await db.Grandchild_checklist.Where(temp => temp.is_active.Equals(true)).ToListAsync();


      List<GrandChildCheckListViewModel> ListViewModel = new List<GrandChildCheckListViewModel>();

      if (allGrandChildCheckList.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var form in allGrandChildCheckList)
      {

        ListViewModel.Add(new GrandChildCheckListViewModel()
        {
          Gc_description = form.gc_description,
          Gc_id = form.gc_id,
          Gc_child_key = form.gc_child_key,
          Gc_bool_status = form.gc_bool_status,
          Gc_added_by = form.gc_added_by,
          Gc_date_added = form.gc_date_added,
          Updated_at = form.updated_at,
          Updated_by = form.updated_by,
          Deactivated_at = form.deactivated_at,
          Deactivated_by = form.deactivated_by,
          Is_active = form.is_active,
          Is_manual = form.is_manual


        });
      }
      return Ok(ListViewModel);


    }



    [HttpPut]
    [Route("api/grandchild_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<GrandChildCheckList>> Put([FromBody] GrandChildCheckList GrandChildRequestParam)
    {

      var ParentCheckListDataInfo = await db.Grandchild_checklist
        .Where(temp => temp.gc_description == GrandChildRequestParam.gc_description).ToListAsync();

      if (ParentCheckListDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }


      var CheckChildForeignKey = await db.Child_checklist.Where(temp => temp.cc_id == Convert.ToInt32(GrandChildRequestParam.gc_child_key)
    ).ToListAsync();

      if (CheckChildForeignKey.Count > 0)
      {

      }
      else
      {
        return BadRequest(new { message = "Child key(" + GrandChildRequestParam.gc_id + ") is not registered on the system" });
      }


      GrandChildCheckList existingDataStatus = await db.Grandchild_checklist.Where(temp => temp.gc_id == GrandChildRequestParam.gc_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.gc_description = GrandChildRequestParam.gc_description;
        existingDataStatus.updated_at = DateTime.Now.ToString();
        existingDataStatus.updated_by = GrandChildRequestParam.updated_by;
        existingDataStatus.is_manual = GrandChildRequestParam.is_manual;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }


    [HttpPut]
    [Route("api/grandchild_checklist/deactivate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<GrandChildCheckList>> PutDeactivate([FromBody] GrandChildCheckList GrandChildRequestParam)
    {

   
      GrandChildCheckList existingDataStatus = await db.Grandchild_checklist.Where(temp => temp.gc_id == GrandChildRequestParam.gc_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.is_active = false;
        existingDataStatus.deactivated_at = DateTime.Now.ToString();
        existingDataStatus.deactivated_by = GrandChildRequestParam.deactivated_by;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }


    [HttpPut]
    [Route("api/grandchild_checklist/activate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<GrandChildCheckList>> PutActivate([FromBody] GrandChildCheckList GrandChildRequestParam)
    {
      GrandChildCheckList existingDataStatus = await db.Grandchild_checklist.Where(temp => temp.gc_id == GrandChildRequestParam.gc_id).FirstOrDefaultAsync();
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
    [Route("api/grandchild_checklist/search/{parent_fk}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetGrandChild(string parent_fk)
    {

      List<GrandChildCheckList> allGrandChildCheckList = await db.Grandchild_checklist.Where(temp => temp.is_active.Equals(true) && temp.gc_child_key.Contains(parent_fk)).ToListAsync();


      List<GrandChildCheckListViewModel> ListViewModel = new List<GrandChildCheckListViewModel>();

      if (allGrandChildCheckList.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var form in allGrandChildCheckList)
      {

        ListViewModel.Add(new GrandChildCheckListViewModel()
        {
          Gc_description = form.gc_description,
          Gc_id = form.gc_id,
          Gc_child_key = form.gc_child_key,
          Gc_bool_status = form.gc_bool_status,
          Gc_added_by = form.gc_added_by,
          Gc_date_added = form.gc_date_added,
          Updated_at = form.updated_at,
          Updated_by = form.updated_by,
          Deactivated_at = form.deactivated_at,
          Deactivated_by = form.deactivated_by,
          Is_active = form.is_active,
          Is_manual = form.is_manual


        });
      }
      return Ok(ListViewModel);


    }







    [HttpPost]
    [Route("api/grandchild_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Post([FromBody] GrandChildCheckList ChildRequestParam)
    {

      //var  sample[];
      //sample = asasa;

      //// Step 1: create list.
      //List<string> list = new List<string>();
      //list.Add("one");
      //list.Add("two");
      //list.Add("three");
      //list.Add("four");
      //list.Add("five");

      //// Step 2: convert to string array.
      //string[] array = list.ToArray();


      //return Ok(list);

      // Step 1: create list.
      //List<string> list = new List<string>();
      //list.Add("one");
      //list.Add("two");
      //list.Add("three");
      //list.Add("four");
      //list.Add("five");

      //// Step 2: convert to string array.
      //string[] array = list.ToArray();




      if (ChildRequestParam.gc_child_key == null || ChildRequestParam.gc_child_key == "")
      {
        return BadRequest(new { message = "Fill up the required fields" });
      }


      var CheckChildForeignKey = await db.Child_checklist.Where(temp => temp.cc_id.ToString() == ChildRequestParam.gc_child_key
      ).ToListAsync();

      if (CheckChildForeignKey.Count > 0)
      {

      }
      else
      {
        return BadRequest(new { message = "Child key(" + ChildRequestParam.gc_child_key + ") is not registered on the system" });
      }


      var ChildCheckListDataInfo = await db.Grandchild_checklist.Where(temp => temp.gc_description == ChildRequestParam.gc_description
      ).ToListAsync();

      if (ChildCheckListDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }


      db.Grandchild_checklist.Add(ChildRequestParam);
      await db.SaveChangesAsync();

      GrandChildCheckList existingProject = await db.Grandchild_checklist.Where(temp => temp.gc_id == ChildRequestParam.gc_id).FirstOrDefaultAsync();

      GrandChildCheckListViewModel ChildViewModel = new GrandChildCheckListViewModel()
      {

        Gc_id = existingProject.gc_id,
        Gc_description = existingProject.gc_description,
        Gc_child_key = existingProject.gc_child_key,
        Gc_bool_status = existingProject.gc_bool_status,
        Is_active = existingProject.is_active,
        Gc_added_by = existingProject.gc_added_by,
        Gc_date_added = existingProject.gc_date_added,
        Is_manual = existingProject.is_manual
      };

      return Ok(ChildViewModel);

    }





  }
}
