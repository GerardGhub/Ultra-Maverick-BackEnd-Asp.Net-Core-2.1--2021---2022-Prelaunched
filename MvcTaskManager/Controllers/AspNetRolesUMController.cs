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
    public async Task<ApplicationRole> Put([FromBody] ApplicationRole Role)
    {
      ApplicationRole existingRoles = await db.ApplicationRoles.Where(temp => temp.Id == Role.Id).FirstOrDefaultAsync();
      if (existingRoles != null)
      {
        existingRoles.Name = Role.Name;
        //existingRejectedStatus.is_active = rejectstats.is_active;
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
