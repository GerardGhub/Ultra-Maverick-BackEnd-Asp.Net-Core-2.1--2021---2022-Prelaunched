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

    public async Task<IActionResult> Get()
    {
      List<RMPoSummaryCancelledStats> tblCancelledPoStatus = await db.CancelledPOTransactionStatus.ToListAsync();
      return Ok(tblCancelledPoStatus);
    }

    [HttpGet]
    [Route("api/CancelledPOTransactionStatus/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetByRejectID(int StatusID)
    {
      RMPoSummaryCancelledStats tblCancelledPO = await db.CancelledPOTransactionStatus.Where(temp => temp.id == StatusID).FirstOrDefaultAsync();
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
    public async Task<RMPoSummaryCancelledStats> Post([FromBody] RMPoSummaryCancelledStats RMStatusdata)
    {
      db.CancelledPOTransactionStatus.Add(RMStatusdata);
     await db.SaveChangesAsync();

      RMPoSummaryCancelledStats existingData = await db.CancelledPOTransactionStatus.Where(temp => temp.id == RMStatusdata.id).FirstOrDefaultAsync();
      return RMStatusdata;
    }

    [HttpPut]
    [Route("api/CancelledPOTransactionStatus")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<RMPoSummaryCancelledStats> Put([FromBody] RMPoSummaryCancelledStats RMstats)
    {
      RMPoSummaryCancelledStats existingDataStatus = await db.CancelledPOTransactionStatus.Where(temp => temp.id == RMstats.id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.status_name = RMstats.status_name;
        existingDataStatus.is_active = RMstats.is_active;
       await db.SaveChangesAsync();
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
    public async Task<int> Delete(int ID)
    {
      RMPoSummaryCancelledStats existingDataStatus = await db.CancelledPOTransactionStatus.Where(temp => temp.id == ID).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        db.CancelledPOTransactionStatus.Remove(existingDataStatus);
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
