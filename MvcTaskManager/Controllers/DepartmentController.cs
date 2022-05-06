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
  public class DepartmentController : Controller
  {
    
    private ApplicationDbContext db;
    public DepartmentController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/department")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Get()
    {
      //List<Department> tblRejectedStatuses = await db.Department.ToListAsync();
      //return Ok(tblRejectedStatuses);

      List<Department> departments =  await db.Department.Where(temp => temp.is_active.Equals(true)).ToListAsync();


      List<DepartmentViewModel> projectsViewModel = new List<DepartmentViewModel>();
      foreach (var dept in departments)
      {
        
        projectsViewModel.Add(new DepartmentViewModel()
        {
          Department_id = dept.department_id,
          Department_name = dept.department_name,
          Created_by = dept.created_by,
          Created_at = dept.created_at,
          Updated_at = dept.updated_at,
          Updated_by = dept.updated_by,
          Is_active = dept.is_active,
          Primary_user_id = dept.primary_user_id,
          Location_id = dept.location_id
          
        });
      }
      return Ok(projectsViewModel);


    }

  }
}
