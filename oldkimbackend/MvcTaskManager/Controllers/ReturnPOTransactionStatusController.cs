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
  public class ReturnPOTransactionStatusController : Controller
  {
    private ApplicationDbContext db;
    public ReturnPOTransactionStatusController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/ReturnPOTransactionStatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public IActionResult Get()
    {
      List<RMReturnPOTransactionStatus> tblReturnedPoStatus = db.ReturnPOTransactionStatus.ToList();
      return Ok(tblReturnedPoStatus);
    }

    [HttpGet]
    [Route("api/ReturnPOTransactionStatus/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetByRejectID(int StatusID)
    {
      RMReturnPOTransactionStatus tblReturnPO = db.ReturnPOTransactionStatus.Where(temp => temp.id == StatusID).FirstOrDefault();
      if (tblReturnPO != null)
      {
        return Ok(tblReturnPO);
      }
      else
        return NoContent();
    }

    [HttpPost]
    [Route("api/ReturnPOTransactionStatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public RMReturnPOTransactionStatus Post([FromBody] RMReturnPOTransactionStatus RMStatusdata)
    {
      db.ReturnPOTransactionStatus.Add(RMStatusdata);
      db.SaveChanges();

      RMReturnPOTransactionStatus existingData = db.ReturnPOTransactionStatus.Where(temp => temp.id == RMStatusdata.id).FirstOrDefault();
      return RMStatusdata;
    }

    [HttpPut]
    [Route("api/ReturnPOTransactionStatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public RMReturnPOTransactionStatus Put([FromBody] RMReturnPOTransactionStatus RMstats)
    {
      RMReturnPOTransactionStatus existingDataStatus = db.ReturnPOTransactionStatus.Where(temp => temp.id == RMstats.id).FirstOrDefault();
      if (existingDataStatus != null)
      {
        existingDataStatus.status_name = RMstats.status_name;
        existingDataStatus.is_active = RMstats.is_active;
        db.SaveChanges();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }

    [HttpDelete]
    [Route("api/ReturnPOTransactionStatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public int Delete(int ID)
    {
      RMReturnPOTransactionStatus existingDataStatus = db.ReturnPOTransactionStatus.Where(temp => temp.id == ID).FirstOrDefault();
      if (existingDataStatus != null)
      {
        db.ReturnPOTransactionStatus.Remove(existingDataStatus);
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
