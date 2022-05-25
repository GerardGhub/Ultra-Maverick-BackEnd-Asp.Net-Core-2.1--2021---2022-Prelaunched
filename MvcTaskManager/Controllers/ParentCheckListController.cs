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




    [HttpPost]
    [Route("api/parent_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Post([FromBody] ParentCheckList materialRequest)
    {

      var ParentCheckListDataInfo = await db.parent_checklist.Where(temp => temp.parent_chck_details == materialRequest.parent_chck_details
      ).ToListAsync();

      if (ParentCheckListDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }


      db.parent_checklist.Add(materialRequest);
      await db.SaveChangesAsync();

      ParentCheckList existingProject = await db.parent_checklist.Where(temp => temp.parent_chck_id == materialRequest.parent_chck_id).FirstOrDefaultAsync();




      ParentCheckListViewModel MRISViewModel = new ParentCheckListViewModel()
      {

        Parent_chck_id = existingProject.parent_chck_id,
        Parent_chck_details = existingProject.parent_chck_details,
        Parent_chck_added_by = existingProject.parent_chck_added_by,
        Parent_chck_date_added = existingProject.parent_chck_date_added,
        Is_active = existingProject.is_active


      };

      return Ok(MRISViewModel);

    }



    [HttpGet]
    [Route("api/parent_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> GetMaterialOrder()
    {

      List<ParentCheckList> allmrs = await db.parent_checklist.Where(temp => temp.is_active.Equals(true)).ToListAsync();


      List<ParentCheckListViewModel> MaterialRequestViewModel = new List<ParentCheckListViewModel>();

      if (allmrs.Count > 0)
      {

      }
      else
      {
        return NoContent();
      }

      foreach (var material in allmrs)
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

