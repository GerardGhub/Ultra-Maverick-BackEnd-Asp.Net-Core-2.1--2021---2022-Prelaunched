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
  public class InternalOrderActivationRemarksController : Controller
  {
    private ApplicationDbContext db;
    public InternalOrderActivationRemarksController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/internal_mrs_cancelled_reason")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Get()
    {
      List<InternalOrderActivationRemarks> MrsActivationRemarks =
      await db.Internal_order_activation_remarks
      .Where(temp => temp.soar_is_active.Equals(true) && temp.soar_type.Contains("Cancel")).ToListAsync();
      List<InternalOrderActivationRemarksViewModel> projectsViewModel = new List<InternalOrderActivationRemarksViewModel>();
      foreach (var dept in MrsActivationRemarks)
      {

        projectsViewModel.Add(new InternalOrderActivationRemarksViewModel()
        {
          Soar_id = dept.soar_id,
          Soar_desc= dept.soar_desc,
          Soar_type = dept.soar_type,
          Soar_added_by = dept.soar_added_by,
          Soar_date_added = dept.soar_date_added,
          Soar_is_active = dept.soar_is_active,
          Soar_updated_by = dept.soar_updated_by,
          Soar_updated_date = dept.soar_updated_date

        });
      }
      return Ok(projectsViewModel);


    }




    [HttpGet]
    [Route("api/internal_mrs_returned_reason")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> GetReturn()
    {
      List<InternalOrderActivationRemarks> MrsActivationRemarks =
      await db.Internal_order_activation_remarks
      .Where(temp => temp.soar_is_active.Equals(true) && temp.soar_type.Contains("Return")).ToListAsync();
      List<InternalOrderActivationRemarksViewModel> projectsViewModel = new List<InternalOrderActivationRemarksViewModel>();
      foreach (var dept in MrsActivationRemarks)
      {

        projectsViewModel.Add(new InternalOrderActivationRemarksViewModel()
        {
          Soar_id = dept.soar_id,
          Soar_desc = dept.soar_desc,
          Soar_type = dept.soar_type,
          Soar_added_by = dept.soar_added_by,
          Soar_date_added = dept.soar_date_added,
          Soar_is_active = dept.soar_is_active,
          Soar_updated_by = dept.soar_updated_by,
          Soar_updated_date = dept.soar_updated_date

        });
      }
      return Ok(projectsViewModel);


    }



  }
}
