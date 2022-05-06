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
  public class LocationController : Controller
  {
    private ApplicationDbContext db;
    public LocationController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/location")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Get()
    {
      //List<tblLocation> tblRejectedStatuses = await db.Location.ToListAsync();
      //return Ok(tblRejectedStatuses);
      List<tblLocation> departments = await db.Location.Where(temp => temp.is_active.Equals(true)).ToListAsync();
      List<LocationViewModel> projectsViewModel = new List<LocationViewModel>();
      foreach (var dept in departments)
      {

        projectsViewModel.Add(new LocationViewModel()
        {
          Location_id = dept.location_id,
          Location_name = dept.location_name,
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
