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
  public class MainMenusController : Controller
  {
    private ApplicationDbContext db;

    public MainMenusController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/MainMenus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get()
    {
      List<MainMenus> MainMenu = await db.MainMenus.ToListAsync();
      return Ok(MainMenu);
    }


    [HttpPost]
    [Route("api/MainMenus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MainMenus> Post([FromBody] MainMenus menu)
    {
      menu.DateAdded = DateTime.Now;
      menu.Isactive = true;
      menu.isactivereference = "Active";
      db.MainMenus.Add(menu);
      await db.SaveChangesAsync();

      MainMenus existingData = await db.MainMenus.Where(temp => temp.Id == menu.Id).FirstOrDefaultAsync();
      return menu;
    }


    [HttpPut]
    [Route("api/MainMenus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MainMenus> Put([FromBody] MainMenus menu)
    {
      menu.ModifiedDate = DateTime.Now;

      if (menu.isactivereference == "Active")
      {
        menu.Isactive = true;
      }
      else
      {
        menu.Isactive = false;
      }

      MainMenus existingDataStatus = await db.MainMenus.Where(temp => temp.Id == menu.Id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Mainmodulename = menu.Mainmodulename;
        existingDataStatus.ModifiedDate = menu.ModifiedDate;
        existingDataStatus.ModifiedBy = menu.ModifiedBy;
        existingDataStatus.Isactive = menu.Isactive;
        existingDataStatus.isactivereference = menu.isactivereference;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }


    [HttpPut]
    [Route("api/MainMenus/Activate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MainMenus> PutActivate([FromBody] MainMenus menu)
    {
      menu.ModifiedDate = DateTime.Now;
      MainMenus existingDataStatus = await db.MainMenus.Where(temp => temp.Id == menu.Id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Isactive = true;
        existingDataStatus.ModifiedDate = menu.ModifiedDate;
        existingDataStatus.ModifiedBy = menu.ModifiedBy;
        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }


    [HttpPut]
    [Route("api/MainMenus/Deactivate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<MainMenus> PutDeactivate([FromBody] MainMenus menu)
    {
      menu.ModifiedDate = DateTime.Now;
      MainMenus existingDataStatus = await db.MainMenus.Where(temp => temp.Id == menu.Id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.Isactive = false;
        existingDataStatus.ModifiedDate = menu.ModifiedDate;
        existingDataStatus.ModifiedBy = menu.ModifiedBy;
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
