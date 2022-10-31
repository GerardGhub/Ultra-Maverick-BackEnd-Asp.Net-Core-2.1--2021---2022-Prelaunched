using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
      var result = await (from RoleModule in db.RoleModules

                          join Module in db.Modules on RoleModule.Moduleid equals Module.Id
                          join Role in db.ApplicationRoles on RoleModule.RoleId equals Role.Id
                          join MainMenu in db.MainMenus on Module.Mainmenuid equals MainMenu.Id


               

                          where Role.Name == RoleId

                                               && Module.Isactive.Equals(true)
                                               && MainMenu.Isactive.Equals(true)
                                               && RoleModule.Isactive.Equals(false)

                          select new
                          {
                            RoleModule.Id,
                            RoleModule.RoleId,
                            RoleModule.Moduleid,
                            RoleModule.Isactive,
                            Module.Submenuname,
                            Module.Modulename,
                            Role.Name
                           
                          })

                      .ToListAsync();

     
        return Ok(result);



    }


    [HttpGet]
    [Route("api/RoleModules/RoleId/{RoleId}/{MainModuleId}")]
    public async Task<ActionResult<RoleModules>> GetByRoleID(string RoleId, int MainModuleId)
    {
      var result = await (from RoleModule in db.RoleModules
                            //join Modules in db.Modules on RoleModule.Mainmoduleidentity equals Modules.Mainmenuid
                            ////join Modules in db.Modules on RoleModule.ModuleId equals Modules.Mainmenuid

                          join Modules in db.Modules on RoleModule.Moduleid equals Modules.Id
                          join MainMenu in db.MainMenus on RoleModule.Mainmoduleidentity equals MainMenu.Id into ps2 from MainMenu in ps2.DefaultIfEmpty()
                          join Role in db.Roles on RoleModule.RoleId equals Role.Id into ps
                          from Role in ps.DefaultIfEmpty()
                          where
                          //&& RoleModule.ModuleId  == ModuleId
                          RoleModule.Mainmoduleidentity == MainModuleId
                          //&& RoleModule.Isactive.Equals(true)
                          && Modules.Isactive.Equals(true)
                          && MainMenu.Isactive.Equals(true)
                          && RoleModule.RoleId == RoleId
                          select new
                          {
                            //RoleModule.Id,
                            RoleModule.RoleId,
                            RoleModule.Moduleid,
                            RoleModule.Isactive,
                            Modules.Submenuname,
                            Modules.Modulename,
                            //Modules.Id,
                            RoleModule.Id,
                            MainMenu.Mainmodulename,
                            Role.Name,
                            RoleModule.Mainmoduleidentity
                          

                          })

                      .ToListAsync();

      //if (result.Count > 0)
      //{
        //return BadRequest("A");
        return Ok(result);
      //}
      //else
      //{
      //  return BadRequest("A");
      //}


    }


    [HttpGet]
    [Route("api/RoleModules/RoleId/Admin/{RoleId}/{MainModuleId}")]
    public async Task<ActionResult<RoleModules>> GetByRoleAdminID(string RoleId, int MainModuleId)
    {
      var result = await (from RoleModule in db.RoleModules
                            //join Modules in db.Modules on RoleModule.Mainmoduleidentity equals Modules.Mainmenuid
                            ////join Modules in db.Modules on RoleModule.ModuleId equals Modules.Mainmenuid

                          join Modules in db.Modules on RoleModule.Moduleid equals Modules.Id
                          join MainMenu in db.MainMenus on RoleModule.Mainmoduleidentity equals MainMenu.Id into ps2
                          from MainMenu in ps2.DefaultIfEmpty()
                          join Role in db.ApplicationRoles on RoleModule.RoleId equals Role.Id into ps
                    
                          from Role in ps.DefaultIfEmpty()

                          where
                          //&& RoleModule.ModuleId  == ModuleId
                          RoleModule.Mainmoduleidentity == MainModuleId
                          //&& RoleModule.Isactive.Equals(true)
                          && Modules.Isactive.Equals(true)
                          && MainMenu.Isactive.Equals(true)
                          && RoleModule.RoleId == "b48c6444-e3d5-4177-ba85-66c35796171d"
                          
                          select new
                          {
                            //RoleModule.Id,
                            RoleModule.RoleId,
                            RoleModule.Moduleid,
                            RoleModule.Isactive,
                            Modules.Submenuname,
                            Modules.Modulename,
                            //Modules.Id,
                            RoleModule.Id,
                            MainMenu.Mainmodulename,
                            Role.Name,
                            RoleModule.Mainmoduleidentity


                          })

                      .ToListAsync();

      if (result.Count > 0)
      {
        //return BadRequest("A");
        return Ok(result);
      }
      else
      {
        //return BadRequest("Axx");
        var result2 = await (from RoleModule in db.RoleModules

                             join Modules in db.Modules on RoleModule.Moduleid equals Modules.Id
                             join Role in db.ApplicationRoles on RoleModule.RoleId equals Role.Id

                             join MainMenu in db.MainMenus on RoleModule.Mainmoduleidentity equals MainMenu.Id
                             where
                            Role.Id == "b48c6444-e3d5-4177-ba85-66c35796171d"
                            &&
                            RoleModule.Mainmoduleidentity == MainModuleId
                            //&& RoleModule.Isactive.Equals(true)
                            && Modules.Isactive.Equals(true)
                            && MainMenu.Isactive.Equals(true)

                             select new
                             {
                               //RoleModule.Id,
                               RoleModule.RoleId,
                               RoleModule.Moduleid,
                               RoleModule.Isactive,
                               Modules.Submenuname,
                               Modules.Modulename,
                               Modules.Id,
                               MainMenu.Mainmodulename,
                               Role.Name


                             })

                .ToListAsync();

        return Ok(result2);
      }





    }


    [HttpGet]
    [Route("api/RoleModules/RoleId/Sample/{RoleId}/{ModuleId}")]
    public async Task<ActionResult<RoleModules>> GetByBaseRoleID(string RoleId, int ModuleId)
    {
      var result = await (from RoleModule in db.RoleModules
                          join Modules in db.Modules on RoleModule.Moduleid equals Modules.Mainmenuid
                          join Role in db.Roles on RoleModule.RoleId equals Role.Id
                          join MainMenu in db.MainMenus on Modules.Mainmenuid equals MainMenu.Id

                          where

                          Role.Id == RoleId
                    &&
                    RoleModule.Moduleid == ModuleId
                          //&& RoleModule.Isactive.Equals(true)
                          && Modules.Isactive.Equals(true)
                          && MainMenu.Isactive.Equals(true)

                          select new
                          {
                            //RoleModule.Id,
                            RoleModule.RoleId,
                            RoleModule.Moduleid,
                            RoleModule.Isactive,
                            Modules.Submenuname,
                            Modules.Modulename,
                            Modules.Id,
                            MainMenu.Mainmodulename,
                            Role.Name


                          })

                      .ToListAsync();

      //if (result.Count > 0 )
      //{
      //  //return BadRequest("A");
      //}
      //else
      //{

      //}

      return Ok(result);


    }


    [HttpPost]
    [Route("api/RoleModules")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<RoleModules> Put([FromBody] RoleModules Role)
    {


      Role.Isactive= true;
        await db.SaveChangesAsync();

      RoleModules existingRoles = await db.RoleModules.Where(temp => temp.Id == Role.Id).FirstOrDefaultAsync();

      return Role;

    }



    [HttpPut]
    [Route("api/RoleModules")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<RoleModules> PutActivate([FromBody] RoleModules Role)
    {

      RoleModules existingRoles = await db.RoleModules.Where(temp => temp.Id == Role.Id).FirstOrDefaultAsync();
      if (existingRoles != null)
      {
        existingRoles.Isactive = true;
        existingRoles.Modifieddate = DateTime.Now;
        existingRoles.Modifiedby = Role.Modifiedby;
        await db.SaveChangesAsync();



        
        //RoleModules validateRoleIdAndModuleId = await db.RoleModules.Where(temp => temp.RoleId == Role.RoleId
        //&& temp.ModuleId == Role.ModuleId).FirstOrDefaultAsync();
        //if (validateRoleIdAndModuleId != null)
        //{
        //}
        //else
        //{
        //  RoleModules roleModules = new RoleModules();
        //  roleModules.Isactive = true;
        //  roleModules.RoleId = Role.RoleId;
        //  roleModules.DateAdded = DateTime.Now;
        //  roleModules.Addedby = Role.Modifiedby;
        //  roleModules.ModuleId = Role.ModuleId;
        //  roleModules.Mainmoduleidentity = Role.Mainmoduleidentity;
        //  db.RoleModules.Add(roleModules);
        //  await db.SaveChangesAsync();
        //}

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


        RoleModules validateRoleIdAndModuleId = await db.RoleModules.Where(temp => temp.RoleId == Role.RoleId
        && temp.Moduleid == Role.Moduleid).FirstOrDefaultAsync();
        if (validateRoleIdAndModuleId != null)
        {
        }
        else
        {
          RoleModules roleModules = new RoleModules();
          roleModules.Isactive = false;
          roleModules.RoleId = Role.RoleId;
          roleModules.DateAdded = DateTime.Now;
          roleModules.Addedby = Role.Modifiedby;
          roleModules.Moduleid = Role.Moduleid;
          roleModules.Mainmoduleidentity = Role.Mainmoduleidentity;
          db.RoleModules.Add(roleModules);
          await db.SaveChangesAsync();
        }
        //RoleModules roleModules = new RoleModules();
        //roleModules.Isactive = false;
        //roleModules.RoleId = Role.RoleId;
        //roleModules.ModuleId = Role.ModuleId;
        //db.RoleModules.Add(roleModules);
        //await db.SaveChangesAsync();

        return existingRoles;
      }
      else
      {
        return null;
      }
    }




  }




}
