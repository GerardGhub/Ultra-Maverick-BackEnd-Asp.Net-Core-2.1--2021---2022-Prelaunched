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
  public class EmployeesController : Controller
  {
    private ApplicationDbContext db;
  public EmployeesController(ApplicationDbContext db)
  {
    this.db = db;
  }

  [HttpGet]
  [Route("api/employees")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

  public async Task<IActionResult> Get()
  {

    List<Employee> employees = await db.Employees.Include("Department").Include("DepartmentUnit").Where(temp => temp.is_active.Equals(true)).ToListAsync();


    List<EmployeeViewModel> EmployeeViewModel = new List<EmployeeViewModel>();
    foreach (var emp in employees)
    {

      EmployeeViewModel.Add(new EmployeeViewModel()
      {
        Id = emp.id,
        Emp_id = emp.emp_id,
        First_name = emp.first_name,
        Last_name = emp.last_name,
        Department = emp.department,
        Sub_unit = emp.sub_unit,
        Is_active = emp.is_active,
        Department_id = emp.Department.department_id.ToString(),
        DepartmentUnit_id = emp.DepartmentUnit.unit_id.ToString()



      });
    }
    return Ok(EmployeeViewModel);


  }

}
}
