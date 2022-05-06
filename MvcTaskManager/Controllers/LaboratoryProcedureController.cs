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
  public class LaboratoryProcedureController : Controller
  {
    private ApplicationDbContext db;
    public LaboratoryProcedureController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/LaboratoryProcedure")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Get()
    {
      List<LaboratoryProcedure> tblLabProc = db.laboratory_procedure.ToList();
      return Ok(tblLabProc);
    }


    [HttpGet]
    [Route("api/LaboratoryProcedure/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetByLabID(int LabID)
    {
      int LaboratoryIdentity = LabID;

      LaboratoryProcedure tblLabProc = db.laboratory_procedure.Where(temp => temp.lab_id == LaboratoryIdentity).FirstOrDefault();
      if (tblLabProc != null)
      {
        return Ok(tblLabProc);
      }
      else
        return NoContent();
    }

    [HttpPost]
    [Route("api/LaboratoryProcedure")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public LaboratoryProcedure Post([FromBody] LaboratoryProcedure LabProc)
    {
      db.laboratory_procedure.Add(LabProc);
      db.SaveChanges();

      LaboratoryProcedure existingData = db.laboratory_procedure.Where(temp => temp.lab_id == LabProc.lab_id).FirstOrDefault();
      return LabProc;
    }

    [HttpPut]
    [Route("api/LaboratoryProcedure")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public LaboratoryProcedure Put([FromBody] LaboratoryProcedure labProc)
    {
      LaboratoryProcedure existingDataStatus = db.laboratory_procedure.Where(temp => temp.lab_id == labProc.lab_id).FirstOrDefault();
      if (existingDataStatus != null)
      {
        existingDataStatus.lab_description = labProc.lab_description;
        existingDataStatus.is_active_status = labProc.is_active_status;
        existingDataStatus.updated_at = labProc.updated_at;
        existingDataStatus.updated_by = labProc.updated_by;
        db.SaveChanges();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }

    [HttpDelete]
    [Route("api/LaboratoryProcedure")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public int Delete(int ID)
    {
      LaboratoryProcedure existingDataStatus = db.laboratory_procedure.Where(temp => temp.lab_id == ID).FirstOrDefault();
      if (existingDataStatus != null)
      {
        db.laboratory_procedure.Remove(existingDataStatus);
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
