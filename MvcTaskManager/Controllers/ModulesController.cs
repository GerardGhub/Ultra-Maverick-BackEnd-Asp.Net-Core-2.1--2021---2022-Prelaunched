using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.CodeAnalysis;

namespace MvcTaskManager.Controllers
{
  public class ModulesController : Controller
  {
    private ApplicationDbContext db;

    public ModulesController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/Modules")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get()
    {
      //List<Modules> Modules = await db.Modules.ToListAsync();
      //return Ok(Modules);

      var result = await (from Moduleskey in db.Modules
                          join MainMenusKey in db.MainMenus on Moduleskey.Mainmenuid equals MainMenusKey.Id
              

                          //where Moduleskey.Isactive.Equals(true)
                          //&& MainMenusKey.Equals(true)

                          select new
                          {
                            Moduleskey.Id,
                            Moduleskey.Mainmenuid,
                            Moduleskey.Submenuname,
                            Moduleskey.Modulename,
                            Moduleskey.DateAdded,
                            Moduleskey.Isactive,
                            Moduleskey.isactivereference,
                            Moduleskey.ModifiedBy,
                            Moduleskey.ModifiedDate,
                            Moduleskey.Reason,
                            Moduleskey.ModuleStatus,
                            Moduleskey.AddedBy,
                            MainMenusKey.Mainmodulename

                          })

                .ToListAsync();

      return Ok(result);
    }


    [HttpPost]
    [Route("api/Modules")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Modules> Post([FromBody] Modules module)
    {

      //var GetUserRole = await db.ApplicationRoles.Where(src => src.Isactive.Equals(true)).ToListAsync();


      //RoleModules roleModules = new RoleModules();

      //foreach (var item in GetUserRole)
      //{

      //  roleModules.Isactive = true;
      //  roleModules.RoleId = item.Id;
      //  roleModules.ModuleId = module.Mainmenuid;
      //  db.RoleModules.Add(roleModules);
      //  await db.SaveChangesAsync();




      //}
      //db.RoleModules.Remove(roleModules);

      //this.Sample();
      ////this.Sample2();
      //return module;

      module.DateAdded = DateTime.Now;
      module.Isactive = true;
      module.isactivereference = "Active";
      db.Modules.Add(module);
      await db.SaveChangesAsync();

      Modules existingData = await db.Modules
     .Where(temp => temp.Id == module.Id)
     .FirstOrDefaultAsync();
      return module;
    }

    private void Sample()
    {
      RoleModules roleModules = new RoleModules();

      roleModules.Isactive = true;
      roleModules.RoleId = "33";
      roleModules.ModuleId = 222;
      db.RoleModules.Add(roleModules);
      db.SaveChangesAsync();

    }
    private void Sample2()
    {
      RoleModules roleModules1 = new RoleModules();

      roleModules1.Isactive = true;
      roleModules1.RoleId = "332";
      roleModules1.ModuleId = 222;
      db.RoleModules.Add(roleModules1);
      db.SaveChangesAsync();

    }


    [HttpPut]
    [Route("api/Modules")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<Modules> Put([FromBody] Modules module)
    {
      module.ModifiedDate = DateTime.Now;

      if (module.isactivereference == "Active")
      {
        module.Isactive = true;
      }
      else
      {
        module.Isactive = false;
      }



      Modules existingDataStatus = await db.Modules
        .Where(temp => temp.Id == module.Id)
        .FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Mainmenuid = module.Mainmenuid;
        existingDataStatus.Submenuname = module.Submenuname;
        existingDataStatus.Modulename = module.Modulename;
        existingDataStatus.ModifiedBy = module.ModifiedBy;
        existingDataStatus.ModifiedDate = DateTime.Now;
        existingDataStatus.Isactive = module.Isactive;
        existingDataStatus.isactivereference = module.isactivereference;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }




  }
}
