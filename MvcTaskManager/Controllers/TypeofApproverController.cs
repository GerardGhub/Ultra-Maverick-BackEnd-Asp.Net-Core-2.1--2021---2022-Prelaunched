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
  public class TypeofApproverController : Controller
  {
    private ApplicationDbContext db;
    public TypeofApproverController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/typeofmrsapprover")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Get()
    {  
      List<TypeofApprover> typeofApprovers = await db.TypeofApprover.Where(temp => temp.is_active.Equals(true)).ToListAsync();
      List<TypeofApproverViewModel> projectsViewModel = new List<TypeofApproverViewModel>();
      foreach (var dept in typeofApprovers)
      {

        projectsViewModel.Add(new TypeofApproverViewModel()
        {
          Approver_id = dept.approver_id,
          Type_of_approver = dept.type_of_approver,
          Created_at = dept.created_at,
          Created_by = dept.created_by,
          Updated_at = dept.updated_at,
          Updated_by = dept.updated_by,
          Is_active = dept.is_active

        });
      }
      return Ok(projectsViewModel);


    }

  }


}
