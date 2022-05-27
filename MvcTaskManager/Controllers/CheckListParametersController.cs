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


    [HttpGet]
    [Route("api/checklist_paramaters")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetParameterCheckList()
    {

      List<CheckListParameters> allParametersCheckList =
        await db.Checklist_paramaters.Where(temp => temp.is_active.Equals(true)).ToListAsync();


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
          Cp_description = form.cp_description,
          Cp_params_id = form.cp_params_id,
          Cp_gchild_key = form.cp_gchild_key,
          Cp_gchild_po_number = form.cp_gchild_po_number,
          Cp_bool_status = form.cp_bool_status,
          Cp_added_by = form.cp_added_by,
          Cp_date_added = form.cp_date_added,
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
