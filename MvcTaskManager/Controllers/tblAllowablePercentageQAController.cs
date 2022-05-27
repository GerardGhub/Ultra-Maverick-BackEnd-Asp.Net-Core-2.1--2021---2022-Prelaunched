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
  public class tblAllowablePercentageQAController : Controller
  {
    private ApplicationDbContext db;
    public tblAllowablePercentageQAController(ApplicationDbContext db)
    {
      this.db = db;
    }


    [HttpGet]
    [Route("api/tblAllowablePercentageQA")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get()
    {
      List<AllowablePercentageQAModel> tblAllowablePercentages = await db.TblAllowablePercentageQA.ToListAsync();
      return Ok(tblAllowablePercentages);
    }


    [HttpGet]
    [Route("api/tblAllowablePercentageQA/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetByRejectID(int AllowPercentageID)
    {
      AllowablePercentageQAModel AllowablePercentageQA = await db.TblAllowablePercentageQA.Where(temp => temp.p_id == AllowPercentageID).FirstOrDefaultAsync();
      if (AllowablePercentageQA != null)
      {
        return Ok(AllowablePercentageQA);
      }
      else
        return NoContent();
    }



    [HttpPost]
    [Route("api/tblAllowablePercentageQA")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<AllowablePercentageQAModel> Post([FromBody] AllowablePercentageQAModel AllowablePercentage)
    {
      db.TblAllowablePercentageQA.Add(AllowablePercentage);
      await db.SaveChangesAsync();

      AllowablePercentageQAModel existingData = await db.TblAllowablePercentageQA.Where(temp => temp.p_id == AllowablePercentage.p_id).FirstOrDefaultAsync();
      return AllowablePercentage;
    }




    [HttpPut]
    [Route("api/tblAllowablePercentageQA")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<AllowablePercentageQAModel> Put([FromBody] AllowablePercentageQAModel AllowablePercentage)
    {
      AllowablePercentageQAModel existingAllowablePercentage = await db.TblAllowablePercentageQA.Where(temp => temp.p_id == AllowablePercentage.p_id).FirstOrDefaultAsync();
      if (existingAllowablePercentage != null)
      {
        existingAllowablePercentage.p_allowable_percentage = AllowablePercentage.p_allowable_percentage;
        existingAllowablePercentage.p_date_modified = AllowablePercentage.p_date_modified;
        existingAllowablePercentage.p_modified_by = AllowablePercentage.p_modified_by;
        await db.SaveChangesAsync();
        return existingAllowablePercentage;
      }
      else
      {
        return null;
      }
    }






  }
}
