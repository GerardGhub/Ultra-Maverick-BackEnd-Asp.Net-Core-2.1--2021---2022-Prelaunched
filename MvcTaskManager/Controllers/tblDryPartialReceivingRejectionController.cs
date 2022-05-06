using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using MvcTaskManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Controllers
{
  public class tblDryPartialReceivingRejectionController : Controller
  {
    private ApplicationDbContext db;

    public tblDryPartialReceivingRejectionController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/tblDryPartialReceivingRejection")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public IActionResult Get()
    {
      string data_is_pending = "1";
      List<tblDryPartialReceivingRejectionModel> tblDryPartialReceivingRejections = db.tblDryPartialReceivingRejection.Where(temp => temp.Is_pending.Contains(data_is_pending)).ToList();
      return Ok(tblDryPartialReceivingRejections);
    }


    [HttpGet]
    [Route("api/tblDryPartialReceivingRejection/search/{searchby}/{searchtext}/{searchindex}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Search(string searchBy, string searchText, string searchIndex)
    {

      string data_is_pending = "1";
      string is_activated = "1";
      List<tblDryPartialReceivingRejectionModel> projects = null;

      string PoNumberValue = searchText;
      string RejectIndexes = searchIndex;
      if (searchBy == "Po_number")

        projects = db.tblDryPartialReceivingRejection.Where(temp => temp.Is_active.Contains(is_activated) && temp.Is_pending.Contains(data_is_pending) && temp.Po_number.ToString().Contains(PoNumberValue) && temp.Projection_identity.ToString().Contains(RejectIndexes)).ToList();


      List<WarehouseRejectStatusViewModel> WarehouseRejectStatusContructor = new List<WarehouseRejectStatusViewModel>();
      foreach (var project in projects)
      {
        WarehouseRejectStatusContructor.Add(new WarehouseRejectStatusViewModel()
        {
      
          Index_id = project.Index_id,
          Po_number = project.Po_number,
          Qty_reject = project.Qty_reject,
          Reject_remarks = project.Reject_remarks,
          Added_by = project.Added_by,
          Is_pending = project.Is_pending
        

        });
      }

      return Ok(WarehouseRejectStatusContructor);
    }



  }

  
}
