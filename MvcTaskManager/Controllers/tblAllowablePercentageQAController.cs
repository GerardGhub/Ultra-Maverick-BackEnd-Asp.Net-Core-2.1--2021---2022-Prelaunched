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
    public IActionResult Get()
    {
      List<AllowablePercentageQAModel> tblAllowablePercentages = db.tblAllowablePercentageQA.ToList();
      return Ok(tblAllowablePercentages);
    }


    [HttpGet]
    [Route("api/tblAllowablePercentageQA/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetByRejectID(int AllowPercentageID)
    {
      AllowablePercentageQAModel AllowablePercentageQA = db.tblAllowablePercentageQA.Where(temp => temp.p_id == AllowPercentageID).FirstOrDefault();
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
    public AllowablePercentageQAModel Post([FromBody] AllowablePercentageQAModel AllowablePercentage)
    {
      db.tblAllowablePercentageQA.Add(AllowablePercentage);
      db.SaveChanges();

      AllowablePercentageQAModel existingData = db.tblAllowablePercentageQA.Where(temp => temp.p_id == AllowablePercentage.p_id).FirstOrDefault();
      return AllowablePercentage;
    }

    [HttpPut]
    [Route("api/tblAllowablePercentageQA")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public AllowablePercentageQAModel Put([FromBody] AllowablePercentageQAModel AllowablePercentage)
    {
      AllowablePercentageQAModel existingAllowablePercentage = db.tblAllowablePercentageQA.Where(temp => temp.p_id == AllowablePercentage.p_id).FirstOrDefault();
      if (existingAllowablePercentage != null)
      {
        existingAllowablePercentage.p_allowable_percentage = AllowablePercentage.p_allowable_percentage;
        existingAllowablePercentage.p_date_modified = AllowablePercentage.p_date_modified;
        existingAllowablePercentage.p_modified_by = AllowablePercentage.p_modified_by;
        db.SaveChanges();
        return existingAllowablePercentage;
      }
      else
      {
        return null;
      }
    }






  }
}
