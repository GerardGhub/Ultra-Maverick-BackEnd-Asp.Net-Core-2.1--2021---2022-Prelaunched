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
  public class LabTestRemarksController : Controller
  {

    private ApplicationDbContext db;
    public LabTestRemarksController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/LabTestRemarks")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get()
    {
      List<LabTestRemarks> TbLLabTestRemarks = await db.Laboratory_test_remarks.ToListAsync();
      return Ok(TbLLabTestRemarks);
    }


    [HttpGet]
    [Route("api/LabTestRemarks/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetByLabID(int LabTestRemarksID)
    {
      int LaboratoryIdentity = LabTestRemarksID;

      LabTestRemarks tblLabTestRemark = await db.Laboratory_test_remarks.Where(temp => temp.lab_remarks_id == LaboratoryIdentity).FirstOrDefaultAsync();
      if (tblLabTestRemark != null)
      {
        return Ok(tblLabTestRemark);
      }
      else
        return NoContent();
    }



    [HttpPost]
    [Route("api/LabTestRemarks")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<LabTestRemarks> Post([FromBody] LabTestRemarks LabRemarks)
    {
      db.Laboratory_test_remarks.Add(LabRemarks);
      await db.SaveChangesAsync();

      LabTestRemarks existingData = await db.Laboratory_test_remarks.Where(temp => temp.lab_remarks_id == LabRemarks.lab_remarks_id).FirstOrDefaultAsync();
      return LabRemarks;
    }

    [HttpPut]
    [Route("api/LabTestRemarks")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<LabTestRemarks> Put([FromBody] LabTestRemarks labTestRemarksProc)
    {
      LabTestRemarks existingDataStatus = await db.Laboratory_test_remarks.Where(temp => temp.lab_remarks_id == labTestRemarksProc.lab_remarks_id).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        existingDataStatus.lab_remarks_description = labTestRemarksProc.lab_remarks_description;
        existingDataStatus.lab_test_remarks_active_status = labTestRemarksProc.lab_test_remarks_active_status;
        existingDataStatus.updated_at = labTestRemarksProc.updated_at;
        existingDataStatus.updated_by = labTestRemarksProc.updated_by;
       await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }

    [HttpDelete]
    [Route("api/LabTestRemarks")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<int> Delete(int ID)
    {
      LabTestRemarks existingDataStatus = await db.Laboratory_test_remarks.Where(temp => temp.lab_remarks_id == ID).FirstOrDefaultAsync();
      if (existingDataStatus != null)
      {
        db.Laboratory_test_remarks.Remove(existingDataStatus);
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
