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
  public class LaboratorySubRemarksController : Controller
  {

    private ApplicationDbContext db;
    public LaboratorySubRemarksController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/LaboratorySubRemark")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Get()
    {
      List<LaboratorySubRemark> TbLLabTestSubRemarks = db.laboratory_sub_remarks.ToList();
      return Ok(TbLLabTestSubRemarks);
    }


    [HttpGet]
    [Route("api/LaboratorySubRemark/searchbyid/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetByLabID(int LabTestSubRemarksID)
    {
      int LaboratoryIdentity = LabTestSubRemarksID;

      LaboratorySubRemark tblLabTestSubRemark = db.laboratory_sub_remarks.Where(temp => temp.lab_sub_remarks_id == LaboratoryIdentity).FirstOrDefault();
      if (tblLabTestSubRemark != null)
      {
        return Ok(tblLabTestSubRemark);
      }
      else
        return NoContent();
    }



    [HttpPost]
    [Route("api/LaboratorySubRemark")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public LaboratorySubRemark Post([FromBody] LaboratorySubRemark LabSubRemarks)
    {
      db.laboratory_sub_remarks.Add(LabSubRemarks);
      db.SaveChanges();

      LaboratorySubRemark existingData = db.laboratory_sub_remarks.Where(temp => temp.lab_sub_remarks_id == LabSubRemarks.lab_sub_remarks_id).FirstOrDefault();
      return LabSubRemarks;
    }

    [HttpPut]
    [Route("api/LaboratorySubRemark")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public LaboratorySubRemark Put([FromBody] LaboratorySubRemark labTestSubRemarksProc)
    {
      LaboratorySubRemark existingDataStatus = db.laboratory_sub_remarks.Where(temp => temp.lab_sub_remarks_id == labTestSubRemarksProc.lab_sub_remarks_id).FirstOrDefault();
      if (existingDataStatus != null)
      {
        existingDataStatus.lab_remarks_desc_parent = labTestSubRemarksProc.lab_remarks_desc_parent;
        existingDataStatus.lab_sub_remarks_description = labTestSubRemarksProc.lab_sub_remarks_description;
        existingDataStatus.updated_at = labTestSubRemarksProc.updated_at;
        existingDataStatus.updated_by = labTestSubRemarksProc.updated_by;
        existingDataStatus.lab_test_remarks_active_status = labTestSubRemarksProc.lab_test_remarks_active_status;
        db.SaveChanges();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }

    [HttpDelete]
    [Route("api/LaboratorySubRemark")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public int Delete(int ID)
    {
      LaboratorySubRemark existingDataStatus = db.laboratory_sub_remarks.Where(temp => temp.lab_sub_remarks_id == ID).FirstOrDefault();
      if (existingDataStatus != null)
      {
        db.laboratory_sub_remarks.Remove(existingDataStatus);
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

