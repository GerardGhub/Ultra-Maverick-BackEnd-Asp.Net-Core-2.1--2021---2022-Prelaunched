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
      List<tblLocation> tblRejectedStatuses = await db.Location.ToListAsync();
      return Ok(tblRejectedStatuses);
    }
  }
}
