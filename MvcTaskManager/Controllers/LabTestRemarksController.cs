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
    public IActionResult Get()
    {
      List<LabTestRemarks> TbLLabTestRemarks = db.laboratory_test_remarks.ToList();
      return Ok(TbLLabTestRemarks);
    }


    [HttpGet]
    [Route("api/LabTestRemarks/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetByLabID(int LabTestRemarksID)
    {
      int LaboratoryIdentity = LabTestRemarksID;

      LabTestRemarks tblLabTestRemark = db.laboratory_test_remarks.Where(temp => temp.lab_remarks_id == LaboratoryIdentity).FirstOrDefault();
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
    public LabTestRemarks Post([FromBody] LabTestRemarks LabRemarks)
    {
      db.laboratory_test_remarks.Add(LabRemarks);
      db.SaveChanges();

      LabTestRemarks existingData = db.laboratory_test_remarks.Where(temp => temp.lab_remarks_id == LabRemarks.lab_remarks_id).FirstOrDefault();
      return LabRemarks;
    }

    [HttpPut]
    [Route("api/LabTestRemarks")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public LabTestRemarks Put([FromBody] LabTestRemarks labTestRemarksProc)
    {
      LabTestRemarks existingDataStatus = db.laboratory_test_remarks.Where(temp => temp.lab_remarks_id == labTestRemarksProc.lab_remarks_id).FirstOrDefault();
      if (existingDataStatus != null)
      {
        existingDataStatus.lab_remarks_description = labTestRemarksProc.lab_remarks_description;
        existingDataStatus.lab_test_remarks_active_status = labTestRemarksProc.lab_test_remarks_active_status;
        existingDataStatus.updated_at = labTestRemarksProc.updated_at;
        existingDataStatus.updated_by = labTestRemarksProc.updated_by;
        db.SaveChanges();
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
    public int Delete(int ID)
    {
      LabTestRemarks existingDataStatus = db.laboratory_test_remarks.Where(temp => temp.lab_remarks_id == ID).FirstOrDefault();
      if (existingDataStatus != null)
      {
        db.laboratory_test_remarks.Remove(existingDataStatus);
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
