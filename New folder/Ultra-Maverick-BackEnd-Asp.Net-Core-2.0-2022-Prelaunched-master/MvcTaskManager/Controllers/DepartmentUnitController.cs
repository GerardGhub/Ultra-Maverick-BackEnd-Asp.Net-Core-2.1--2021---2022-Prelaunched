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
  public class DepartmentUnitController : Controller
  {

    private ApplicationDbContext db;
    public DepartmentUnitController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/unit")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Get()
    {
      //List<Department> tblRejectedStatuses = await db.Department.ToListAsync();
      //return Ok(tblRejectedStatuses);

      List<DepartmentUnit> units = await db.DepartmentUnit.Where(temp => temp.is_active.Equals(true)).ToListAsync();


      List<DepartmentUnitViewModel> departmentUnitViewModel = new List<DepartmentUnitViewModel>();
      foreach (var unity in units)
      {

        departmentUnitViewModel.Add(new DepartmentUnitViewModel()
        {
         Unit_id = unity.unit_id,
          Unit_description = unity.unit_description,
          Department = unity.department,
          Updated_at = unity.updated_at,
          Updated_by = unity.updated_by,
          Created_at = unity.created_at,
          Created_by = unity.created_by,
          Is_active =  unity.is_active
      

        });
      }
      return Ok(departmentUnitViewModel);


    }

  }


}
