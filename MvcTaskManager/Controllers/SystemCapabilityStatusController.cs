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
  public class SystemCapabilityStatusController : Controller
  {
    private ApplicationDbContext db;
    public SystemCapabilityStatusController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/SystemCapabilityStatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<ActionResult> Get()
    {
      List<SystemCapabilityStatus> SystemStatus = await db.system_capability_status.ToListAsync();
      return Ok(SystemStatus);
    }

  }
}
