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
    public async Task<IActionResult> Get()
    {
      List<LaboratoryProcedure> tblLabProc = await db.laboratory_procedure.ToListAsync();
      return Ok(tblLabProc);
    }


    [HttpGet]
    [Route("api/LaboratoryProcedure/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetByLabID(int LabID)
    {
      int LaboratoryIdentity = LabID;

      LaboratoryProcedure tblLabProc = await db.laboratory_procedure.Where(temp => temp.lab_id == LaboratoryIdentity).FirstOrDefaultAsync();
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
    public async Task<LaboratoryProcedure> Post([FromBody] LaboratoryProcedure LabProc)
    {
      db.laboratory_procedure.Add(LabProc);
      await db.SaveChangesAsync();

      LaboratoryProcedure existingData = await db.laboratory_procedure.Where(temp => temp.lab_id == LabProc.lab_id).FirstOrDefaultAsync();
      return LabProc;
    }

    [HttpPut]
    [Route("api/LaboratoryProcedure")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<LaboratoryProcedure> Put([FromBody] LaboratoryProcedure labProc)
    {
      LaboratoryProcedure existingDataStatus = await db.laboratory_procedure.Where(temp => temp.lab_id == labProc.lab_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.lab_description = labProc.lab_description;
        existingDataStatus.is_active_status = labProc.is_active_status;
        existingDataStatus.updated_at = labProc.updated_at;
        existingDataStatus.updated_by = labProc.updated_by;
        await db.SaveChangesAsync();
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
    public async Task<int> Delete(int ID)
    {
      LaboratoryProcedure existingDataStatus = await db.laboratory_procedure.Where(temp => temp.lab_id == ID).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        db.laboratory_procedure.Remove(existingDataStatus);
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
