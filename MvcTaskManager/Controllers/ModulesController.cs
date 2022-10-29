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
using System.Data;
using MvcTaskManager.ViewModels;

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
    public async Task<ActionResult<Modules>> Post([FromBody] Modules module)
    {
      int CountModules = 0;
      //var TotalModules = await db.Modules.ToListAsync();
      //TotalModules = TotalModules.FIls

      //CountModules = TotalModules.Count + 1;

      //return BadRequest(CountModules);


      Modules existingDataStatus = (Modules)await db.Modules
    .Where(temp => temp.Mainmenuid == module.Mainmenuid && temp.Submenuname == module.Submenuname &&
    temp.Modulename == module.Modulename)
    .FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        return BadRequest(new { message = "Data Already Exist" });
      }

      var TotalModules = await db.Modules.ToListAsync();

      CountModules = TotalModules.Count + 1;



      module.DateAdded = DateTime.Now;
      module.Isactive = true;
      module.isactivereference = "Active";
      db.Modules.Add(module);
      await db.SaveChangesAsync();

      Modules existingData = await db.Modules
     .Where(temp => temp.Id == module.Id)
     .FirstOrDefaultAsync();



      RoleModules roleModules = new RoleModules();
      roleModules.Isactive = true;
      roleModules.RoleId = "b48c6444-e3d5-4177-ba85-66c35796171d";
      roleModules.DateAdded = DateTime.Now;
      roleModules.Addedby = module.AddedBy;
      roleModules.ModuleId =CountModules;
      roleModules.Mainmoduleidentity = module.Mainmenuid;
      db.RoleModules.Add(roleModules);
      await db.SaveChangesAsync();

      return module;
    }



    [HttpPut]
    [Route("api/Modules")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<Modules>> Put([FromBody] Modules module)
    {

      Modules requestPayload = (Modules)await db.Modules
    .Where(temp => temp.Mainmenuid == module.Mainmenuid && temp.Submenuname == module.Submenuname &&
    temp.Modulename == module.Modulename && temp.isactivereference == module.isactivereference)
    .FirstOrDefaultAsync();
      if (requestPayload != null)
      {
        return BadRequest(new { message = "Data Already Exist" });
      }



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
