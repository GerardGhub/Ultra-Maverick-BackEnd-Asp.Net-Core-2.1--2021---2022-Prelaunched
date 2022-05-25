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
    public async Task<IActionResult> Post([FromBody] ChildCheckList parentRequestParam)
    {

      if (parentRequestParam.cc_parent_key == null || parentRequestParam.cc_parent_key == ""
        || parentRequestParam.cc_parent_po_number == null || parentRequestParam.cc_parent_po_number == "")
      {
        return BadRequest(new { message = "Fill up the required fields" });
      }


      var CheckParentForeignKey = await db.parent_checklist.Where(temp => temp.parent_chck_id.ToString() == parentRequestParam.cc_parent_key
      ).ToListAsync();

      if (CheckParentForeignKey.Count > 0)
      {

      }
      else
      {
      return BadRequest(new { message = "Parent key(" + parentRequestParam.cc_parent_key + ") is not registered on the system" });
      }


      var ChildCheckListDataInfo = await db.child_checklist.Where(temp => temp.cc_description == parentRequestParam.cc_description
      ).ToListAsync();

      if (ChildCheckListDataInfo.Count > 0)
      {
        return BadRequest(new { message = "You already have a duplicate request check the data to proceed" });
      }


      db.child_checklist.Add(parentRequestParam);
      await db.SaveChangesAsync();

      ChildCheckList existingProject = await db.child_checklist.Where(temp => temp.cc_id == parentRequestParam.cc_id).FirstOrDefaultAsync();

      ChildCheckListViewModel ChildViewModel = new ChildCheckListViewModel()
      {

        Cc_id = existingProject.cc_id,
        Cc_description = existingProject.cc_description,
        Cc_parent_key = existingProject.cc_parent_key,
        Cc_parent_po_number = existingProject.cc_parent_po_number,
        Cc_bool_status = existingProject.cc_bool_status,
        Is_active = existingProject.is_active
      };

      return Ok(ChildViewModel);

    }




    [HttpGet]
    [Route("api/child_checklist")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetMaterialOrder()
    {

      List<ChildCheckList> allChildCheckList = await db.child_checklist.Where(temp => temp.is_active.Equals(true)).ToListAsync();


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
          Cc_parent_key = form.cc_parent_key,
          Cc_parent_po_number = form.cc_parent_po_number,
          Cc_bool_status = form.cc_bool_status,
          Cc_added_by = form.cc_added_by,
          Cc_date_added = form.cc_date_added,
          Updated_at = form.updated_at,
          Updated_by = form.updated_by,
          Deactivated_at = form.deactivated_at,
          Deactivated_by = form.deactivated_by,
          Is_active = form.is_active


        });
      }
      return Ok(ListViewModel);


    }










  }
}
