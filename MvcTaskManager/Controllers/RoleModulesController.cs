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
  public class RoleModulesController : Controller
  {
    private readonly ApplicationDbContext db;
    
    public RoleModulesController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/RoleModules/{RoleId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<ActionResult<RoleModules>> GetByRoleID(string RoleId)
    {
      var result = await (from Parents in db.RoleModules
                          join Modules in db.Modules on Parents.ModuleId equals Modules.Mainmenuid
                          join Role in db.Roles on  Parents.RoleId equals Role.Id
                          join MainMenu in db.MainMenus on  Modules.Mainmenuid equals MainMenu.Id

                          where Role.Name == RoleId
                          && Parents.Isactive.Equals(true)
                          && Modules.Isactive.Equals(true)
                          && MainMenu.Isactive.Equals(true)

                          select new
                          {
                            Parents.Id,
                            Parents.RoleId,
                            Parents.ModuleId,
                            Parents.Isactive,
                            Modules.Submenuname,
                            Modules.Modulename,
                            Role.Name
                           
                          })

                      .ToListAsync();

      return Ok(result);


    }




    }



  
}
