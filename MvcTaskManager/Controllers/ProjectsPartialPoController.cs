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

  public class ProjectsPartialPoController : Controller
  {
    private ApplicationDbContext db;
    public ProjectsPartialPoController(ApplicationDbContext db)
    {
      this.db = db;
    }
    [HttpGet]
    [Route("api/ProjectsPartialPo")]
    public IActionResult Get()
    {
      List<RMProjectsPartialPo> projects = db.ProjectsPartialPo
        .Where(temp => temp.is_activated.Contains("1")
             
      && temp.Is_expired.Contains("0")
      && temp.Is_wh_received != "1"
      || temp.Is_approved_XP.Contains("1")
      || temp.Is_wh_reject_approval.Contains("1")
      && temp.Cancelled_reason == null
      && temp.Is_return_in_qc != 1
      //&& temp.Is_wh_reject_approval == null
      ).ToList();

      int TotalCountofReject = 0;

      List<ProjectViewModel> projectsViewModel = new List<ProjectViewModel>();
      foreach (var project in projects)
      {
        if (project.Is_wh_reject == "")
        {
          project.Is_wh_reject = "0";
        }

        TotalCountofReject = (Convert.ToInt32(project.Total_of_reject_mat)) + (Convert.ToInt32(project.Is_wh_reject));

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
          Total_of_reject_mat = TotalCountofReject.ToString(),

         //Cancelled

          Cancelled_date = project.Cancelled_date,
          Canceled_by = project.Canceled_by,
          Cancelled_reason = project.Cancelled_reason,
          //Returned
          Returned_date = project.Returned_date,
          Returned_by = project.Returned_by,
          Returned_reason = project.Returned_reason,
          //Date Receiving by QA
          QCReceivingDate = project.QCReceivingDate,
          Is_wh_reject = project.Is_wh_reject

        });
      }
      return Ok(projectsViewModel);




    }



    [HttpGet]
    [Route("api/ProjectsPartialPo/WHReject")]
    public IActionResult GetItemRejectAtWH()
    {

      string ProjectIsActivated = "1";
      string GoodRM = "0";
      string Approve = "1";

      List<RMProjectsPartialPo> projects = db.ProjectsPartialPo
        .Where(temp => temp.is_activated.Contains(ProjectIsActivated)
        && temp.Is_expired.Contains(GoodRM)
      && temp.Is_wh_received.Contains(Approve)
      && (Convert.ToInt32(temp.Is_wh_reject) > 0) != temp.Is_wh_reject_approval.Contains(Approve) && temp.Is_wh_reject != "0").ToList();

      List<ProjectsPartialPoViewModel> projectsViewModel = new List<ProjectsPartialPoViewModel>();
      foreach (var project in projects)
      {
        projectsViewModel.Add(new ProjectsPartialPoViewModel()
        {
          ProjectID = project.ProjectID,
          //PrimaryID = project.PrimaryID, // Add Ne
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


          //Cancelled

          Cancelled_date = project.Cancelled_date,
          Canceled_by = project.Canceled_by,
          Cancelled_reason = project.Cancelled_reason,
          //Returned
          Returned_date = project.Returned_date,
          Returned_by = project.Returned_by,
          Returned_reason = project.Returned_reason,
          //Date Receiving by QA
          QCReceivingDate = project.QCReceivingDate,
          Is_wh_received = project.Is_wh_received,
          Is_wh_received_by = project.Is_wh_received_by,
          Is_wh_received_date = project.Is_wh_received_date,
          Is_wh_reject = project.Is_wh_reject


        });
      }
      return Ok(projectsViewModel);




    }


    //Expiration Approval
    [HttpGet]
    [Route("api/ProjectsPartialPo/ExpiryApproval")]
    public IActionResult GetPoNearlyExpiry()
    {
      
      string ProjectIsActivated = "1";
      string ExpiredRM = "1";
    
      List<RMProjectsPartialPo> projects = db.ProjectsPartialPo.Where(temp => temp.is_activated.Contains(ProjectIsActivated)
        && temp.Is_expired.Contains(ExpiredRM) != temp.Is_approved_XP.Contains(ExpiredRM)).ToList();

      List<ProjectViewModel> projectsViewModel = new List<ProjectViewModel>();
      foreach (var project in projects)
      {
        //int dayDiff = (project.Expiration_date_string - DateTime.Now).Days;
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

          //Cancelled

          Cancelled_date = project.Cancelled_date,
          Canceled_by = project.Canceled_by,
          Cancelled_reason = project.Cancelled_reason,
          //Returned
          Returned_date = project.Returned_date,
          Returned_by = project.Returned_by,
          Returned_reason = project.Returned_reason,
          //Date Receiving by QA
          QCReceivingDate = project.QCReceivingDate,
          //Sample

          //DaysBeforeExpired = dayDiff

        });
      }
      return Ok(projectsViewModel);




    }




    [HttpGet]
    [Route("api/ProjectsPartialPo/search/{searchby}/{searchtext}")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Search(string searchBy, string searchText)
    {
        string ProjectIsActivated = "1";
        List<RMProjectsPartialPo> projects = null;
        if (searchBy == "ProjectID")
        projects = db.ProjectsPartialPo.Where(temp => temp.ProjectID.ToString().Contains(searchText)).ToList();
        else if (searchBy == "ProjectName")
        projects = db.ProjectsPartialPo.Where(temp => temp.ProjectName.Contains(searchText)).ToList();
        else if (searchBy == "Po_number")
           
        projects = db.ProjectsPartialPo.Where(temp => temp.Po_number.Contains(searchText) && temp.is_activated.Contains(ProjectIsActivated) != temp.Is_wh_received.Contains(ProjectIsActivated)).ToList();

        else if (searchBy == "item_code")
        projects = db.ProjectsPartialPo.Where(temp => temp.item_code.Contains(searchText)).ToList();
        else if (searchBy == "item_description")
        projects = db.ProjectsPartialPo.Where(temp => temp.item_description.Contains(searchText)).ToList();
        if (searchBy == "DateOfStart")
        projects = db.ProjectsPartialPo.Where(temp => temp.DateOfStart.ToString().Contains(searchText)).ToList();
        if (searchBy == "TeamSize")
        projects = db.ProjectsPartialPo.Where(temp => temp.TeamSize.ToString().Contains(searchText)).ToList();

        List<ProjectViewModel> projectsViewModel = new List<ProjectViewModel>();
        foreach (var project in projects)
        {
        projectsViewModel.Add(new ProjectViewModel() { ProjectID = project.ProjectID, Po_number = project.Po_number, ProjectName = project.ProjectName, TeamSize = project.TeamSize, DateOfStart = project.DateOfStart.ToString("dd/MM/yyyy"), Active = project.Active,

        Status = project.Status, Actual_delivery = project.Actual_delivery});
        }

        return Ok(projectsViewModel);
        }




    [HttpGet]
    [Route("api/ProjectsPartialPo/searchbyprojectid/{ProjectID}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetProjectByProjectIdentity(int ProjectID)
    {
      Project project = db.Projects.Where(temp => temp.ProjectID == ProjectID).FirstOrDefault();
      if (project != null)
      {
        ProjectViewModel projectViewModel = new ProjectViewModel() { ProjectID = project.ProjectID, ProjectName = project.ProjectName, TeamSize = project.TeamSize, DateOfStart = project.DateOfStart.ToString("dd/MM/yyyy"), Active = project.Active,
        
          Status = project.Status };
        return Ok(projectViewModel);
      }
      else
        return new EmptyResult();
    }



    [HttpPost]
    [Route("api/ProjectsPartialPo")]
    //[Authorize]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Post([FromBody] RMProjectsPartialPo project)
    {
    
      project.QCReceivingDate = DateTime.Now.ToString();
      project.is_activated = "1";
      project.Active = true;


      var TotalPartialReceiving = await db.ProjectsPartialPo.Where(temp => temp.Active.Equals(true)).ToListAsync();

      project.ProjectID = TotalPartialReceiving.Count + 1;

      var GetProjectsPoNumber = await db.Projects.Where(src => src.Po_number.Contains(project.Po_number)).ToListAsync();

      foreach (var item in GetProjectsPoNumber)
      {
        project.Projects_FK = item.ProjectID;
      }

   

      db.ProjectsPartialPo.Add(project);
      db.SaveChanges();

      RMProjectsPartialPo existingProject = db.ProjectsPartialPo.Where(temp => temp.ProjectID == project.ProjectID).FirstOrDefault();
      ProjectViewModel projectViewModel = new ProjectViewModel()
      {
        ProjectID = existingProject.ProjectID,
        Projects_FK = existingProject.Projects_FK,
        ProjectName = existingProject.ProjectName,
        DateOfStart = existingProject.DateOfStart.ToString("dd/MM/yyyy"),
        Active = existingProject.Active,
        Supplier = existingProject.Supplier,
        is_activated = existingProject.is_activated,
        item_code = existingProject.item_code,
        item_description = existingProject.item_description,
        Po_number = existingProject.Po_number,
        Po_date = existingProject.Po_date,
        Pr_number = existingProject.Pr_number,
        Pr_date = existingProject.Pr_date,
        Qty_order = existingProject.Qty_order,
        Qty_uom = existingProject.Qty_uom,
        Mfg_date = existingProject.Mfg_date,
        Expiration_date = existingProject.Expiration_date,
        Expected_delivery = existingProject.Expected_delivery,
        Actual_delivery = existingProject.Actual_delivery,
        Actual_remaining_receiving = existingProject.Actual_remaining_receiving,
        Received_by_QA = existingProject.Received_by_QA,
        Status_of_reject_one = existingProject.Status_of_reject_one,
        Status_of_reject_two = existingProject.Status_of_reject_two,
        Status_of_reject_three = existingProject.Status_of_reject_three,
        Count_of_reject_one = existingProject.Count_of_reject_one,
        Count_of_reject_two = existingProject.Count_of_reject_two,
        Count_of_reject_three = existingProject.Count_of_reject_three,
        Total_of_reject_mat = existingProject.Total_of_reject_mat,

        //SECTION 1

        //Insert Date
        QCReceivingDate = existingProject.QCReceivingDate,

        //Expiry Partial
        Days_expiry_setup = existingProject.Days_expiry_setup,
        Is_expired = existingProject.Is_expired,
        //Approval
        Is_approved_XP = existingProject.Is_approved_XP,
        Is_approved_by = existingProject.Is_approved_by,
        Is_approved_date = existingProject.Is_approved_date


      };

      return Ok(projectViewModel);
    }





    //Nearly Expiry Approval     public IActionResult Put([FromBody] Project project)
    [HttpPut]
    [Route("api/ProjectsPartialPo/WhReject/Approval")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult PutApproval([FromBody] Project project)
    {
      RMProjectsPartialPo existingProject = db.ProjectsPartialPo.Where(temp => temp.ProjectID == project.ProjectID).FirstOrDefault();
      if (existingProject != null)
      {


        project.Is_wh_reject_approval = "1";
        //Aproval opf the 
        existingProject.Is_wh_reject_approval = project.Is_wh_reject_approval;
        existingProject.Is_wh_reject_approval_by = project.Is_wh_reject_approval_by;
        existingProject.Is_wh_reject_approval_date = project.Is_wh_reject_approval_date = DateTime.Now.ToString();

        db.SaveChanges();

        RMProjectsPartialPo existingProject2 = db.ProjectsPartialPo.Where(temp => temp.ProjectID == project.ProjectID).FirstOrDefault();
        ProjectViewModel projectViewModel = new ProjectViewModel()
        {
          ProjectID = existingProject2.ProjectID,
          ProjectName = existingProject2.ProjectName,
          TeamSize = existingProject2.TeamSize,

          DateOfStart = existingProject2.DateOfStart.ToString("dd/MM/yyyy"),
          Active = existingProject2.Active,
          is_activated = existingProject2.is_activated,
          Status = existingProject2.Status,

          Supplier = existingProject.Supplier,
          item_code = existingProject.item_code,
          Po_number = existingProject.Po_number,
          Po_date = existingProject.Po_date,
          Pr_number = existingProject.Pr_number,
          Pr_date = existingProject.Pr_date,
          Qty_order = existingProject.Qty_order,
          Qty_uom = existingProject.Qty_uom,
          Mfg_date = existingProject.Mfg_date,
          Expiration_date = existingProject.Expiration_date
        ,
          Expected_delivery = existingProject.Expected_delivery,
          Actual_delivery = existingProject.Actual_delivery,
          Actual_remaining_receiving = existingProject.Actual_remaining_receiving,
          Received_by_QA = existingProject.Received_by_QA,
          Status_of_reject_one = existingProject.Status_of_reject_one,
          Status_of_reject_two = existingProject.Status_of_reject_two,
          Count_of_reject_one = existingProject.Count_of_reject_one,
          Count_of_reject_two = existingProject.Count_of_reject_two,
          Count_of_reject_three = existingProject.Count_of_reject_three,
          Total_of_reject_mat = existingProject.Total_of_reject_mat,
          //SECTION 1
          //A

          //Cancelled Po Summary
          Cancelled_date = existingProject.Cancelled_date,
          Canceled_by = existingProject.Canceled_by,
          Cancelled_reason = existingProject.Cancelled_reason,

          //Returned PO Summary
          Returned_date = existingProject.Returned_date,
          Returned_by = existingProject.Returned_by,
          Returned_reason = existingProject.Returned_reason,

          //QC Receiving Date
          //QCReceivingDate = existingProject.QCReceivingDate
          Is_approved_XP = existingProject.Is_approved_XP,
          Is_approved_by = existingProject.Is_approved_by,
          Is_approved_date = existingProject.Is_approved_date,
          //Rejection Approval of QC Supervisor
          Is_wh_reject_approval = existingProject.Is_wh_reject_approval,
          Is_wh_reject_approval_by = existingProject.Is_wh_reject_approval_by,
          Is_wh_reject_approval_date = existingProject.Is_wh_reject_approval_date

        };
        return Ok(projectViewModel);
      }
      else
      {
        return null;
      }
    }




    //Nearly Expiry Approval     public IActionResult Put([FromBody] Project project)
    [HttpPut]
    [Route("api/ProjectsPartialPo/WhReject/Approval/Return")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult PutApprovalReturn([FromBody] Project project)
    {
      RMProjectsPartialPo existingProject = db.ProjectsPartialPo.Where(temp => temp.ProjectID == project.ProjectID).FirstOrDefault();
      if (existingProject != null)
      {


        project.Is_wh_reject_approval = "1";

        //Aproval opf the 
        existingProject.Is_wh_reject_approval = project.Is_wh_reject_approval;
        existingProject.Is_wh_reject_approval_by = project.Is_wh_reject_approval_by;
        existingProject.Is_wh_reject_approval_date = project.Is_wh_reject_approval_date = DateTime.Now.ToString();
        existingProject.Is_return_in_qc = 1;
        db.SaveChanges();

        RMProjectsPartialPo existingProject2 = db.ProjectsPartialPo.Where(temp => temp.ProjectID == project.ProjectID).FirstOrDefault();
        ProjectViewModel projectViewModel = new ProjectViewModel()
        {
          ProjectID = existingProject2.ProjectID,
          ProjectName = existingProject2.ProjectName,
          TeamSize = existingProject2.TeamSize,

          DateOfStart = existingProject2.DateOfStart.ToString("dd/MM/yyyy"),
          Active = existingProject2.Active,
          is_activated = existingProject2.is_activated,
          Status = existingProject2.Status,

          Supplier = existingProject.Supplier,
          item_code = existingProject.item_code,
          Po_number = existingProject.Po_number,
          Po_date = existingProject.Po_date,
          Pr_number = existingProject.Pr_number,
          Pr_date = existingProject.Pr_date,
          Qty_order = existingProject.Qty_order,
          Qty_uom = existingProject.Qty_uom,
          Mfg_date = existingProject.Mfg_date,
          Expiration_date = existingProject.Expiration_date
        ,
          Expected_delivery = existingProject.Expected_delivery,
          Actual_delivery = existingProject.Actual_delivery,
          Actual_remaining_receiving = existingProject.Actual_remaining_receiving,
          Received_by_QA = existingProject.Received_by_QA,
          Status_of_reject_one = existingProject.Status_of_reject_one,
          Status_of_reject_two = existingProject.Status_of_reject_two,
          Count_of_reject_one = existingProject.Count_of_reject_one,
          Count_of_reject_two = existingProject.Count_of_reject_two,
          Count_of_reject_three = existingProject.Count_of_reject_three,
          Total_of_reject_mat = existingProject.Total_of_reject_mat,
     

          //Cancelled Po Summary
          Cancelled_date = existingProject.Cancelled_date,
          Canceled_by = existingProject.Canceled_by,
          Cancelled_reason = existingProject.Cancelled_reason,

          //Returned PO Summary
          Returned_date = existingProject.Returned_date,
          Returned_by = existingProject.Returned_by,
          Returned_reason = existingProject.Returned_reason,

          //QC Receiving Date
          //QCReceivingDate = existingProject.QCReceivingDate
          Is_approved_XP = existingProject.Is_approved_XP,
          Is_approved_by = existingProject.Is_approved_by,
          Is_approved_date = existingProject.Is_approved_date,
          //Rejection Approval of QC Supervisor
          Is_wh_reject_approval = existingProject.Is_wh_reject_approval,
          Is_wh_reject_approval_by = existingProject.Is_wh_reject_approval_by,
          Is_wh_reject_approval_date = existingProject.Is_wh_reject_approval_date,
          Is_return_in_qc = existingProject.Is_return_in_qc.ToString()
          

        };
        return Ok(projectViewModel);
      }
      else
      {
        return null;
      }
    }


    //Nearly Expiry Approval     public IActionResult Put([FromBody] Project project)
    [HttpPut]
    [Route("api/ProjectsPartialPo")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Put([FromBody] Project project)
    {
      RMProjectsPartialPo existingProject = db.ProjectsPartialPo.Where(temp => temp.ProjectID == project.ProjectID).FirstOrDefault();
      if (existingProject != null)
      {
        project.Is_approved_XP = "1";

        //Aproval opf the
        //existingProject.Is_return_in_qc = 1; remove muna 12.21/2022
        existingProject.Is_approved_XP = project.Is_approved_XP;
        existingProject.Is_approved_by = project.Is_approved_by;
        existingProject.Is_approved_date = project.Is_approved_date = DateTime.UtcNow.ToString();

        db.SaveChanges();

        RMProjectsPartialPo existingProject2 = db.ProjectsPartialPo.Where(temp => temp.ProjectID == project.ProjectID).FirstOrDefault();
        ProjectViewModel projectViewModel = new ProjectViewModel()
        {
          ProjectID = existingProject2.ProjectID,
          ProjectName = existingProject2.ProjectName,
          TeamSize = existingProject2.TeamSize,

          DateOfStart = existingProject2.DateOfStart.ToString("dd/MM/yyyy"),
          Active = existingProject2.Active,
          is_activated = existingProject2.is_activated,
          Status = existingProject2.Status,
          
          Supplier = existingProject.Supplier,
          item_code = existingProject.item_code,
          Po_number = existingProject.Po_number,
          Po_date = existingProject.Po_date,
          Pr_number = existingProject.Pr_number,
          Pr_date = existingProject.Pr_date,
          Qty_order = existingProject.Qty_order,
          Qty_uom = existingProject.Qty_uom,
          Mfg_date = existingProject.Mfg_date,
          Expiration_date = existingProject.Expiration_date
        ,
          Expected_delivery = existingProject.Expected_delivery,
          Actual_delivery = existingProject.Actual_delivery,
          Actual_remaining_receiving = existingProject.Actual_remaining_receiving,
          Received_by_QA = existingProject.Received_by_QA,
          Status_of_reject_one = existingProject.Status_of_reject_one,
          Status_of_reject_two = existingProject.Status_of_reject_two,
          Count_of_reject_one = existingProject.Count_of_reject_one,
          Count_of_reject_two = existingProject.Count_of_reject_two,
          Count_of_reject_three = existingProject.Count_of_reject_three,
          Total_of_reject_mat = existingProject.Total_of_reject_mat,
          //Cancelled Po Summary
          Cancelled_date = existingProject.Cancelled_date,
          Canceled_by = existingProject.Canceled_by,
          Cancelled_reason = existingProject.Cancelled_reason,
          //Returned PO Summary
          Returned_date = existingProject.Returned_date,
          Returned_by = existingProject.Returned_by,
          Returned_reason = existingProject.Returned_reason,
          //QC Receiving Date
          Is_approved_XP = existingProject.Is_approved_XP,
          Is_approved_by = existingProject.Is_approved_by,
          Is_approved_date = existingProject.Is_approved_date,
          //Rejection Approval of QC Supervisor
          Is_wh_reject_approval = existingProject.Is_wh_reject_approval,
          Is_wh_reject_approval_by = existingProject.Is_wh_reject_approval_by,
          Is_wh_reject_approval_date = existingProject.Is_wh_reject_approval_date

        };
        return Ok(projectViewModel);
      }
      else
      {
        return null;
      }
    }




    [HttpPut]
    [Route("api/ProjectsPartialPoCancel")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult PutCancelData([FromBody] Project project)
    {
      RMProjectsPartialPo existingProject = db.ProjectsPartialPo.Where(temp => temp.ProjectID == project.ProjectID).FirstOrDefault();
      if (existingProject != null)
      {
        //Aproval opf the
        existingProject.is_activated = project.is_activated;
        existingProject.Cancelled_reason = project.Cancelled_reason;
        existingProject.Canceled_by = project.Canceled_by;
        existingProject.Cancelled_date = project.Cancelled_date = DateTime.Now.ToString();


        db.SaveChanges();

        RMProjectsPartialPo existingProject2 = db.ProjectsPartialPo.Where(temp => temp.ProjectID == project.ProjectID).FirstOrDefault();
        ProjectViewModel projectViewModel = new ProjectViewModel()
        {
          ProjectID = existingProject2.ProjectID,
          ProjectName = existingProject2.ProjectName,
          TeamSize = existingProject2.TeamSize,

          DateOfStart = existingProject2.DateOfStart.ToString("dd/MM/yyyy"),
          Active = existingProject2.Active,
          is_activated = existingProject2.is_activated,
          Status = existingProject2.Status,

          Supplier = existingProject.Supplier,
          item_code = existingProject.item_code,
          Po_number = existingProject.Po_number,
          Po_date = existingProject.Po_date,
          Pr_number = existingProject.Pr_number,
          Pr_date = existingProject.Pr_date,
          Qty_order = existingProject.Qty_order,
          Qty_uom = existingProject.Qty_uom,
          Mfg_date = existingProject.Mfg_date,
          Expiration_date = existingProject.Expiration_date
        ,
          Expected_delivery = existingProject.Expected_delivery,
          Actual_delivery = existingProject.Actual_delivery,
          Actual_remaining_receiving = existingProject.Actual_remaining_receiving,
          Received_by_QA = existingProject.Received_by_QA,
          Status_of_reject_one = existingProject.Status_of_reject_one,
          Status_of_reject_two = existingProject.Status_of_reject_two,
          Count_of_reject_one = existingProject.Count_of_reject_one,
          Count_of_reject_two = existingProject.Count_of_reject_two,
          Count_of_reject_three = existingProject.Count_of_reject_three,
          Total_of_reject_mat = existingProject.Total_of_reject_mat,


          //Cancelled Po Summary
          Cancelled_date = existingProject.Cancelled_date,
          Canceled_by = existingProject.Canceled_by,
          Cancelled_reason = existingProject.Cancelled_reason,

          //Returned PO Summary
          Returned_date = existingProject.Returned_date,
          Returned_by = existingProject.Returned_by,
          Returned_reason = existingProject.Returned_reason,

          //QC Receiving Date
          Is_approved_XP = existingProject.Is_approved_XP,
          Is_approved_by = existingProject.Is_approved_by,
          Is_approved_date = existingProject.Is_approved_date,
          //Rejection Approval of QC Supervisor
          Is_wh_reject_approval = existingProject.Is_wh_reject_approval,
          Is_wh_reject_approval_by = existingProject.Is_wh_reject_approval_by,
          Is_wh_reject_approval_date = existingProject.Is_wh_reject_approval_date

        };
        return Ok(projectViewModel);
      }
      else
      {
        return null;
      }
    }




  }
}
