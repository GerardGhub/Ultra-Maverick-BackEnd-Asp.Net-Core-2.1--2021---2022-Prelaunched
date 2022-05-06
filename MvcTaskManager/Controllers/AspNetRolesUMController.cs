using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public IActionResult Get()
    {

      List<ApplicationRole> AspNetUsers = db.ApplicationRoles.ToList();
      return Ok(AspNetUsers);
    }



    [HttpGet]
    [Route("api/AspNetRoles/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetByRejectID(string roleID)
    {
      ApplicationRole AspUser = db.ApplicationRoles.Where(temp => temp.Id == roleID).FirstOrDefault();
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
    public ApplicationRole Put([FromBody] ApplicationRole Role)
    {
      ApplicationRole existingRoles = db.ApplicationRoles.Where(temp => temp.Id == Role.Id).FirstOrDefault();
      if (existingRoles != null)
      {
        existingRoles.Name = Role.Name;
        //existingRejectedStatus.is_active = rejectstats.is_active;
        db.SaveChanges();
        return existingRoles;
      }
      else
      {
        return null;
      }
    }

    [HttpDelete]
    [Route("api/tblrejectedstatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public int Delete(int ID)
    {
      TblRejectedStats existingRejectedStatus = db.TblRejectedStatus.Where(temp => temp.id == ID).FirstOrDefault();
      if (existingRejectedStatus != null)
      {
        db.TblRejectedStatus.Remove(existingRejectedStatus);
        db.SaveChanges();
        return ID;
      }
      else
      {
        return -1;
      }
    }




  }
}
