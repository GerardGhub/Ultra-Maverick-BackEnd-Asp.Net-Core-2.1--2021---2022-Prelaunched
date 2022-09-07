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
  public class PositionController : Controller
  {
    private ApplicationDbContext db;
    public PositionController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/position")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Get()
    {
      //List<Department> tblRejectedStatuses = await db.Department.ToListAsync();
      //return Ok(tblRejectedStatuses);

      List<Position> positions = await db.Position.Where(temp => temp.is_active.Equals(true)).ToListAsync();


      List<PositionViewModel> dogStylePositionViewModel = new List<PositionViewModel>();
      foreach (var post in positions)
      {

        dogStylePositionViewModel.Add(new PositionViewModel()
        {
          Position_id = post.position_id,
          Position_name = post.position_name,
          Department_id = post.department_id,
          Created_by = post.created_by,
          Created_at = post.created_at,
          Modified_by = post.modified_by,
          Modified_date = post.modified_date,
          Is_active = post.is_active
 

        });
      }
      return Ok(dogStylePositionViewModel);


    }

  }




}
