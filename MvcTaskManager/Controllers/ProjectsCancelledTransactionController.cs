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
  public class ProjectsCancelledTransactionController : Controller
  {

    private ApplicationDbContext db;

    public ProjectsCancelledTransactionController(ApplicationDbContext db)
    {
      this.db = db;
    }

    [HttpGet]
    [Route("api/projects/cancelled")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Get()
    {
      //System.Threading.Thread.Sleep(1000);
      //List<Project> projects = db.Projects.Include("ClientLocation").ToList();
      string ProjectIsActivated = "0";


      List<Project> projects = db.Projects.Where(temp => temp.is_activated.Contains(ProjectIsActivated)).ToList();
      //List<Project> projects = db.Projects.Include("ClientLocation").Where(temp => temp.ProjectName == ProjectName).ToList();
      List<ProjectViewModel> projectsViewModel = new List<ProjectViewModel>();
      foreach (var project in projects)
      {
        projectsViewModel.Add(new ProjectViewModel()
        {
          ProjectID = project.ProjectID,
          ProjectName = project.ProjectName,
          TeamSize = project.TeamSize,
          DateOfStart = project.DateOfStart.ToString("dd/MM/yyyy"),
          Active = project.Active,
      
          Status = project.Status,
          is_activated = project.is_activated,
          Supplier = project.Supplier,
          item_code = project.item_code,
          Po_number = project.Po_number,
          Po_date = project.Po_date,
          item_description = project.item_description,
          Pr_number = project.Pr_number,
          Pr_date = project.Pr_date,
          Qty_order = project.Qty_order,
          Qty_uom = project.Qty_uom,
          Mfg_date = project.Mfg_date,
          Expiration_date = project.Expiration_date,
          Expected_delivery = project.Expected_delivery,
          Actual_delivery = project.Actual_delivery,
          Actual_remaining_receiving = project.Actual_remaining_receiving,
          Received_by_QA = project.Received_by_QA,
          Status_of_reject_one = project.Status_of_reject_one,
          Status_of_reject_two = project.Status_of_reject_two,
          Status_of_reject_three = project.Status_of_reject_three,
          Count_of_reject_one = project.Count_of_reject_one,
          Count_of_reject_two = project.Count_of_reject_two,
          Count_of_reject_three = project.Count_of_reject_three,
          Total_of_reject_mat = project.Total_of_reject_mat,
          //SECTION 1
          //A


          //Cancelled Raw Mats
       Cancelled_date = project.Cancelled_date,
       Canceled_by = project.Canceled_by,
       Cancelled_reason = project.Cancelled_reason


        });
      }
      return Ok(projectsViewModel);




    }


    //
  }
}
