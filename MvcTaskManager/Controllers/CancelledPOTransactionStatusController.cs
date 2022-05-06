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
  public class CancelledPOTransactionStatusController : Controller
  {
    private ApplicationDbContext db;
    public CancelledPOTransactionStatusController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/CancelledPOTransactionStatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public IActionResult Get()
    {
      List<RMPoSummaryCancelledStats> tblCancelledPoStatus = db.CancelledPOTransactionStatus.ToList();
      return Ok(tblCancelledPoStatus);
    }

    [HttpGet]
    [Route("api/CancelledPOTransactionStatus/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetByRejectID(int StatusID)
    {
      RMPoSummaryCancelledStats tblCancelledPO = db.CancelledPOTransactionStatus.Where(temp => temp.id == StatusID).FirstOrDefault();
      if (tblCancelledPO != null)
      {
        return Ok(tblCancelledPO);
      }
      else
        return NoContent();
    }

    [HttpPost]
    [Route("api/CancelledPOTransactionStatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public RMPoSummaryCancelledStats Post([FromBody] RMPoSummaryCancelledStats RMStatusdata)
    {
      db.CancelledPOTransactionStatus.Add(RMStatusdata);
      db.SaveChanges();

      RMPoSummaryCancelledStats existingData = db.CancelledPOTransactionStatus.Where(temp => temp.id == RMStatusdata.id).FirstOrDefault();
      return RMStatusdata;
    }

    [HttpPut]
    [Route("api/CancelledPOTransactionStatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public RMPoSummaryCancelledStats Put([FromBody] RMPoSummaryCancelledStats RMstats)
    {
      RMPoSummaryCancelledStats existingDataStatus = db.CancelledPOTransactionStatus.Where(temp => temp.id == RMstats.id).FirstOrDefault();
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
    [Route("api/CancelledPOTransactionStatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public int Delete(int ID)
    {
      RMPoSummaryCancelledStats existingDataStatus = db.CancelledPOTransactionStatus.Where(temp => temp.id == ID).FirstOrDefault();
      if (existingDataStatus != null)
      {
        db.CancelledPOTransactionStatus.Remove(existingDataStatus);
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
