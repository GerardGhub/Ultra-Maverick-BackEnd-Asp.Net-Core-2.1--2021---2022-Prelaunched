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

  public class tblRejectedStatusController : Controller
  {
    private ApplicationDbContext db;
    public tblRejectedStatusController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/tblrejectedstatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public async Task<IActionResult> Get()
    {
      List<TblRejectedStats> tblRejectedStatuses = await db.TblRejectedStatus.ToListAsync();
      return Ok(tblRejectedStatuses);
    }


    [HttpGet]
    [Route("api/tblrejectedstatus/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetByRejectID(int RejectionID)
    {
      TblRejectedStats tblRejectedStatuses = await db.TblRejectedStatus.Where(temp => temp.id == RejectionID).FirstOrDefaultAsync();
      if (tblRejectedStatuses != null)
      {
        return Ok(tblRejectedStatuses);
      }
      else
        return NoContent();
    }


    [HttpPost]
    [Route("api/tblrejectedstatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<TblRejectedStats> Post([FromBody] TblRejectedStats rejectedStatusdata)
    {
      db.TblRejectedStatus.Add(rejectedStatusdata);
      db.SaveChanges();

      TblRejectedStats  existingData = await db.TblRejectedStatus.Where(temp => temp.id == rejectedStatusdata.id).FirstOrDefaultAsync();
      return rejectedStatusdata;
    }




    [HttpPut]
    [Route("api/tblrejectedstatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<TblRejectedStats> Put([FromBody] TblRejectedStats rejectstats)
    {
      TblRejectedStats existingRejectedStatus = await db.TblRejectedStatus.Where(temp => temp.id == rejectstats.id).FirstOrDefaultAsync();
      if (existingRejectedStatus != null)
      {
        existingRejectedStatus.reject_status_name = rejectstats.reject_status_name;
        existingRejectedStatus.is_active = rejectstats.is_active;
        db.SaveChanges();
        return existingRejectedStatus;
      }
      else
      {
        return null;
      }
    }

    [HttpDelete]
    [Route("api/tblrejectedstatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<int> Delete(int ID)
    {
      TblRejectedStats existingRejectedStatus = await db.TblRejectedStatus.Where(temp => temp.id == ID).FirstOrDefaultAsync();
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
