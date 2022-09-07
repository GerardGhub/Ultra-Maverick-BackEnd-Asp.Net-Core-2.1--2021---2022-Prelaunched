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
  public class CancelledLabTestTransactionStatusController : Controller
  {
  private readonly ApplicationDbContext db;

        public CancelledLabTestTransactionStatusController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("api/CancelledLabTestTransactionStatus")]
        public async Task<IActionResult> GetLabTestCancellationReason()
        {
      List<CancelledLabTestTransactionStatus> CancelledReason =  await this.db.CancelledLabTestTransactionStatus.Where(temp => temp.Is_Active.Equals(true)).ToListAsync();
            return Ok(CancelledReason);
        }



  }
}
