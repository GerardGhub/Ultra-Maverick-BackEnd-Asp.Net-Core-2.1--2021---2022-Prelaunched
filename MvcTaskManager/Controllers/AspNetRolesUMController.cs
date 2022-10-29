using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
  public class AspNetRolesUMController : Controller
  {

    private ApplicationDbContext db;
    public AspNetRolesUMController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/AspNetRoles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Get()
    {

      List<ApplicationRole> AspNetUsers = await db.ApplicationRoles.ToListAsync();
      return Ok(AspNetUsers);
    }


    [HttpPost]
    [Route("api/AspNetRoles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ApplicationRole>> Post([FromBody] ApplicationRole menu)
    {

      ApplicationRole existingDataStatus = (ApplicationRole)await db.Roles
        .Where(temp => temp.Name == menu.Name)
        .FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        return BadRequest(new { message = "Data Already Exist" });
      }

      //   return BadRequest(new { message = "Transaction number is not exist" });


        menu.Isactive = true;
      menu.Isactivereference = "Active";
      menu.Dateadded = DateTime.Now;
      menu.NormalizedName = menu.Name.ToUpper();

      db.ApplicationRoles.Add(menu);
      await db.SaveChangesAsync();

      ApplicationRole existingData = await db.ApplicationRoles.Where(temp => temp.Id == menu.Id).FirstOrDefaultAsync();
      return menu;
    }




    [HttpGet]
    [Route("api/AspNetRoles/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetByRejectID(string roleID)
    {
      ApplicationRole AspUser = await db.ApplicationRoles.Where(temp => temp.Id == roleID).FirstOrDefaultAsync();
      if (AspUser != null)
      {
        return Ok(AspUser);
      }
      else
        return NoContent();
    }



    [HttpPut]
    [Route("api/AspNetRoles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ApplicationRole>> Put([FromBody] ApplicationRole Role)
    {

      ApplicationRole existingDataStatus = (ApplicationRole)await db.Roles
  .Where(temp => temp.Name == Role.Name)
  .FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        return BadRequest(new { message = "Data Already Exist" });
      }




      Role.NormalizedName = Role.Name.ToUpper();
      var Status = (Role.Isactivereference == "Active") ? Role.Isactive = true : Role.Isactive = false;
      ApplicationRole existingRoles = await db.ApplicationRoles.Where(temp => temp.Id == Role.Id).FirstOrDefaultAsync();
      if (existingRoles != null)
      {
        existingRoles.Name = Role.Name;
        existingRoles.NormalizedName = Role.NormalizedName;
        existingRoles.Isactivereference = Role.Isactivereference;
        existingRoles.Isactive = Role.Isactive;
        existingRoles.Modifiedby = Role.Modifiedby;
        existingRoles.Modifieddate = DateTime.Now;
        await db.SaveChangesAsync();
        return existingRoles;
      }
      else
      {
        return null;
      }
    }

    //[HttpDelete]
    //[Route("api/tblrejectedstatus")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //public async Task<int> Delete(int ID)
    //{
    //  TblRejectedStats existingRejectedStatus = await db.TblRejectedStatus.Where(temp => temp.id == ID).FirstOrDefaultAsync();
    //  if (existingRejectedStatus != null)
    //  {
    //    db.TblRejectedStatus.Remove(existingRejectedStatus);
    //    await db.SaveChangesAsync();
    //    return ID;
    //  }
    //  else
    //  {
    //    return -1;
    //  }
    //}




  }
}
