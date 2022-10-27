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

    public async Task<ActionResult<RoleModules>> GetByRoleName(string RoleId)
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


    [HttpGet]
    [Route("api/RoleModules/RoleId/{RoleId}/{ModuleId}")]
    public async Task<ActionResult<RoleModules>> GetByRoleID(string RoleId, int ModuleId)
    {
      var result = await (from RoleModule in db.RoleModules
                          join Modules in db.Modules on RoleModule.ModuleId equals Modules.Mainmenuid
                          join Role in db.Roles on RoleModule.RoleId equals Role.Id
                          join MainMenu in db.MainMenus on Modules.Mainmenuid equals MainMenu.Id

                          where Role.Id ==RoleId
                          && RoleModule.ModuleId  == ModuleId
                          //&& RoleModule.Isactive.Equals(true)
                          && Modules.Isactive.Equals(true)
                          && MainMenu.Isactive.Equals(true)

                          select new
                          {
                            RoleModule.Id,
                            RoleModule.RoleId,
                            RoleModule.ModuleId,
                            RoleModule.Isactive,
                            Modules.Submenuname,
                            Modules.Modulename,
                            MainMenu.Mainmodulename,
                            Role.Name

                          })

                      .ToListAsync();

      return Ok(result);


    }


    [HttpPut]
    [Route("api/RoleModules")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<RoleModules> Put([FromBody] RoleModules Role)
    {

    RoleModules existingRoles = await db.RoleModules.Where(temp => temp.Id == Role.Id).FirstOrDefaultAsync();
      if (existingRoles != null)
      {
        existingRoles.Isactive= false;
        existingRoles.Modifieddate = DateTime.Now;
        existingRoles.Modifiedby = Role.Modifiedby;
        await db.SaveChangesAsync();
        return existingRoles;
      }
      else
      {
        return null;
      }
    }





    [HttpPut]
    [Route("api/RoleModules/Deactivate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<RoleModules> PutDeactivate([FromBody] RoleModules Role)
    {

      RoleModules existingRoles = await db.RoleModules.Where(temp => temp.Id == Role.Id).FirstOrDefaultAsync();
      if (existingRoles != null)
      {
        existingRoles.Isactive = false;
        existingRoles.Modifieddate = DateTime.Now;
        existingRoles.Modifiedby = Role.Modifiedby;
        await db.SaveChangesAsync();
        return existingRoles;
      }
      else
      {
        return null;
      }
    }




  }




}
