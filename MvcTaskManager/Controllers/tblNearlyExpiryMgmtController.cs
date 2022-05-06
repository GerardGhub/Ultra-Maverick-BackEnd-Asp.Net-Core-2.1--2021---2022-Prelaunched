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
  public class tblNearlyExpiryMgmtController : Controller
  {



    private ApplicationDbContext db;
    public tblNearlyExpiryMgmtController(ApplicationDbContext db)
    {
      this.db = db;
    }
    [HttpGet]

    [Route("api/tblNearlyExpiryMgmt")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get()
    {
      List<tblNearlyExpiryMgmtModel> tblNearlyExpiryMgmts = await db.tblNearlyExpiryMgmt.ToListAsync();
      return Ok(tblNearlyExpiryMgmts);
    }


    [HttpGet]
    [Route("api/tblNearlyExpiryMgmt/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetByRejectID(int ID)
    {
      tblNearlyExpiryMgmtModel tblNearlyExpiryMgmtModels = await db.tblNearlyExpiryMgmt.Where(temp => temp.p_id == ID).FirstOrDefaultAsync();
      if (tblNearlyExpiryMgmtModels != null)
      {
        return Ok(tblNearlyExpiryMgmtModels);
      }
      else
        return NoContent();
    }



    [HttpPost]
    [Route("api/tblNearlyExpiryMgmt")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<tblNearlyExpiryMgmtModel> Post([FromBody] tblNearlyExpiryMgmtModel NearlyExpiry)
    {
      db.tblNearlyExpiryMgmt.Add(NearlyExpiry);
      await db.SaveChangesAsync();

      tblNearlyExpiryMgmtModel existingData = await db.tblNearlyExpiryMgmt.Where(temp => temp.p_id == NearlyExpiry.p_id).FirstOrDefaultAsync();
      return NearlyExpiry;
    }

    [HttpPut]
    [Route("api/tblNearlyExpiryMgmt")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<tblNearlyExpiryMgmtModel> Put([FromBody] tblNearlyExpiryMgmtModel NearlyExpiry)
    {
      tblNearlyExpiryMgmtModel existingData = await db.tblNearlyExpiryMgmt.Where(temp => temp.p_id == NearlyExpiry.p_id).FirstOrDefaultAsync();
      if (existingData != null)
      {
        existingData.p_nearly_expiry_desc = NearlyExpiry.p_nearly_expiry_desc;
        existingData.p_date_modified = NearlyExpiry.p_date_modified;
        existingData.p_modified_by = NearlyExpiry.p_modified_by;
        await db.SaveChangesAsync();
        return existingData;
      }
      else
      {
        return null;
      }
    }

    [HttpDelete]
    [Route("api/tblNearlyExpiryMgmt")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<int> Delete(int ID)
    {
      tblNearlyExpiryMgmtModel existingData = await db.tblNearlyExpiryMgmt.Where(temp => temp.p_id == ID).FirstOrDefaultAsync();
      if (existingData != null)
      {
        db.tblNearlyExpiryMgmt.Remove(existingData);
        await db.SaveChangesAsync();
        return ID;
      }
      else
      {
        return -1;
      }
    }




  }

}

