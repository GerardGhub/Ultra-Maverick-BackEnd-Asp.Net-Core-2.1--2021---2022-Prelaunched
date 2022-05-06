using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult Get()
    {
      List<tblNearlyExpiryMgmtModel> tblNearlyExpiryMgmts = db.tblNearlyExpiryMgmt.ToList();
      return Ok(tblNearlyExpiryMgmts);
    }


    [HttpGet]
    [Route("api/tblNearlyExpiryMgmt/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetByRejectID(int ID)
    {
      tblNearlyExpiryMgmtModel tblNearlyExpiryMgmtModels = db.tblNearlyExpiryMgmt.Where(temp => temp.p_id == ID).FirstOrDefault();
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
    public tblNearlyExpiryMgmtModel Post([FromBody] tblNearlyExpiryMgmtModel NearlyExpiry)
    {
      db.tblNearlyExpiryMgmt.Add(NearlyExpiry);
      db.SaveChanges();

      tblNearlyExpiryMgmtModel existingData = db.tblNearlyExpiryMgmt.Where(temp => temp.p_id == NearlyExpiry.p_id).FirstOrDefault();
      return NearlyExpiry;
    }

    [HttpPut]
    [Route("api/tblNearlyExpiryMgmt")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public tblNearlyExpiryMgmtModel Put([FromBody] tblNearlyExpiryMgmtModel NearlyExpiry)
    {
      tblNearlyExpiryMgmtModel existingData = db.tblNearlyExpiryMgmt.Where(temp => temp.p_id == NearlyExpiry.p_id).FirstOrDefault();
      if (existingData != null)
      {
        existingData.p_nearly_expiry_desc = NearlyExpiry.p_nearly_expiry_desc;
        existingData.p_date_modified = NearlyExpiry.p_date_modified;
        existingData.p_modified_by = NearlyExpiry.p_modified_by;
        db.SaveChanges();
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
    public int Delete(int ID)
    {
      tblNearlyExpiryMgmtModel existingData = db.tblNearlyExpiryMgmt.Where(temp => temp.p_id == ID).FirstOrDefault();
      if (existingData != null)
      {
        db.tblNearlyExpiryMgmt.Remove(existingData);
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

