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

  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

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
      //System.Threading.Thread.Sleep(1000);
      //List<Project> projects = db.Projects.Include("ClientLocation").ToList();
      string ProjectIsActivated = "1";
      string GoodRM = "0";
      string Approve = "1";
      //List<Project> projects = db.Projects.Include("ClientLocation").Where(temp => temp.Active.ToString().Contains((char)1)).ToList();

      List<RMProjectsPartialPo> projects = db.ProjectsPartialPo.Where(temp => temp.is_activated.Contains(ProjectIsActivated) && temp.Is_expired.Contains(GoodRM)
      && temp.Is_wh_received != "1" || temp.Is_approved_XP.Contains(Approve)).ToList();


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
          A_delivery_van_desc = project.A_delivery_van_desc,
          A_compliance = project.A_compliance,
          A_remarks = project.A_remarks,

          B_cooling_system_desc = project.B_cooling_system_desc,
          B_compliance = project.B_compliance,
          B_remarks = project.B_remarks,

          C_inner_walls_desc = project.C_inner_walls_desc,
          C_compliance = project.C_compliance,
          C_remarks = project.C_remarks,

          D_plastic_curtains_desc = project.D_plastic_curtains_desc,
          D_compliance = project.D_compliance,
          D_remarks = project.D_remarks,

          E_thereno_pest_desc = project.E_thereno_pest_desc,
          E_compliance = project.E_compliance,
          E_remarks = project.E_remarks,
          //SECTION 2
          //A
          A_clean_company_dos = project.A_clean_company_dos,
          A_compliance_dos = project.A_compliance_dos,
          A_remarks_dos = project.A_remarks_dos,

          B_delivery_staff_symptoms_dos = project.B_delivery_staff_symptoms_dos,
          B_compliance_dos = project.B_compliance_dos,
          B_remarks_dos = project.B_remarks_dos,

          C_inner_walls_clean_dos = project.C_inner_walls_clean_dos,
          C_compliance_dos = project.C_compliance_dos,
          C_remarks_dos = project.C_remarks_dos,

          D_plastic_curtains_dos = project.D_plastic_curtains_dos,
          D_compliance_dos = project.D_compliance_dos,
          D_remarks_dos = project.D_remarks_dos,

          E_no_accessories_dos = project.E_no_accessories_dos,
          E_compliance_dos = project.E_compliance_dos,
          E_remarks_dos = project.E_remarks_dos,

          F_no_pests_sightings_dos = project.F_no_pests_sightings_dos,
          F_remarks_dos = project.F_remarks_dos,
          F_compliance_dos = project.F_compliance_dos,
          //SECTION 3
          //A
          A_pallet_crates_tres = project.A_pallet_crates_tres,
          A_compliance_tres = project.A_compliance_tres,
          A_remarks_tres = project.A_remarks_tres,

          B_product_contamination_tres = project.B_product_contamination_tres,
          B_compliance_tres = project.B_compliance_tres,
          B_remarks_tres = project.B_remarks_tres,

          C_uncessary_items_tres = project.C_uncessary_items_tres,
          C_compliance_tres = project.C_compliance_tres,
          C_remarks_tres = project.C_remarks_tres,

          D_products_cover_tres = project.D_products_cover_tres,
          D_compliance_tres = project.D_compliance_tres,
          D_remarks_tres = project.D_remarks_tres,
          //SECTION 4
          //A
          A_certificate_coa_kwatro_desc = project.A_certificate_coa_kwatro_desc,
          A_compliance_kwatro = project.A_compliance_kwatro,
          A_remarks_kwatro = project.A_remarks_kwatro,
          //B
          B_po_kwatro_desc = project.B_po_kwatro_desc,
          B_compliance_kwatro = project.B_compliance_kwatro,
          B_remarks_kwatro = project.B_remarks_kwatro,
          //C
          C_msds_kwatro_desc = project.C_msds_kwatro_desc,
          C_compliance_kwatro = project.C_compliance_kwatro,
          C_remarks_kwatro = project.C_remarks_kwatro,
          //D
          D_food_grade_desc = project.D_food_grade_desc,
          D_compliance_kwatro = project.D_compliance_kwatro,
          D_remarks_kwatro = project.D_remarks_kwatro,
          //SECTION 5
          //A

          A_qty_received_singko_singko = project.A_qty_received_singko_singko,
          A_compliance_singko = project.A_compliance_singko,
          A_remarks_singko = project.A_remarks_singko,
          //B
          B_mfg_date_desc_singko = project.B_mfg_date_desc_singko,
          B_compliance_singko = project.B_compliance_singko,
          B_remarks_singko = project.B_remarks_singko,
          //C
          C_expirydate_desc_singko = project.C_expirydate_desc_singko,
          C_compliance_singko = project.C_compliance_singko,
          C_remarks_singko = project.C_remarks_singko,
          //D
          D_packaging_desc_singko = project.D_packaging_desc_singko,
          D_compliance_singko = project.D_compliance_singko,
          D_remarks_singko = project.D_remarks_singko,
          //E
          E_no_contaminants_desc_singko = project.E_no_contaminants_desc_singko,
          E_compliance_singko = project.E_compliance_singko,
          E_remarks_singko = project.E_remarks_singko,
          //F
          F_qtyrejected_desc_singko = project.F_qtyrejected_desc_singko,
          F_compliance_singko = project.F_compliance_singko,
          F_remarks_singko = project.F_remarks_singko,
          //G
          G_rejected_reason_desc_singko = project.G_rejected_reason_desc_singko,
          G_compliance_singko = project.G_compliance_singko,
          G_remarks_singko = project.G_remarks_singko,
          //H
          H_lab_sample_desc_singko = project.H_lab_sample_desc_singko,
          H_compliance_singko = project.H_compliance_singko,
          H_remarks_singko = project.H_remarks_singko,

          //Cancelled

          Cancelled_date = project.Cancelled_date,
          Canceled_by = project.Canceled_by,
          Cancelled_reason = project.Cancelled_reason,
          //Returned
          Returned_date = project.Returned_date,
          Returned_by = project.Returned_by,
          Returned_reason = project.Returned_reason,
          //Date Receiving by QA
          QCReceivingDate = project.QCReceivingDate

        });
      }
      return Ok(projectsViewModel);




    }

    //Warehouse Rejection Fetching Data
    [HttpGet]
    [Route("api/ProjectsPartialPo/WHReject")]
    public IActionResult GetItemRejectAtWH()
    {

      string ProjectIsActivated = "1";
      string GoodRM = "0";
      string Approve = "1";

      List<RMProjectsPartialPo> projects = db.ProjectsPartialPo.Where(temp => temp.is_activated.Contains(ProjectIsActivated) && temp.Is_expired.Contains(GoodRM)
      && temp.Is_wh_received.Contains(Approve) && (Convert.ToInt32(temp.Is_wh_reject) > 0) != temp.Is_wh_reject_approval.Contains(Approve) && temp.Is_wh_reject != "0").ToList();

      List<ProjectsPartialPoViewModel> projectsViewModel = new List<ProjectsPartialPoViewModel>();
      foreach (var project in projects)
      {
        projectsViewModel.Add(new ProjectsPartialPoViewModel()
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
          A_delivery_van_desc = project.A_delivery_van_desc,
          A_compliance = project.A_compliance,
          A_remarks = project.A_remarks,

          B_cooling_system_desc = project.B_cooling_system_desc,
          B_compliance = project.B_compliance,
          B_remarks = project.B_remarks,

          C_inner_walls_desc = project.C_inner_walls_desc,
          C_compliance = project.C_compliance,
          C_remarks = project.C_remarks,

          D_plastic_curtains_desc = project.D_plastic_curtains_desc,
          D_compliance = project.D_compliance,
          D_remarks = project.D_remarks,

          E_thereno_pest_desc = project.E_thereno_pest_desc,
          E_compliance = project.E_compliance,
          E_remarks = project.E_remarks,
          //SECTION 2
          //A
          A_clean_company_dos = project.A_clean_company_dos,
          A_compliance_dos = project.A_compliance_dos,
          A_remarks_dos = project.A_remarks_dos,

          B_delivery_staff_symptoms_dos = project.B_delivery_staff_symptoms_dos,
          B_compliance_dos = project.B_compliance_dos,
          B_remarks_dos = project.B_remarks_dos,

          C_inner_walls_clean_dos = project.C_inner_walls_clean_dos,
          C_compliance_dos = project.C_compliance_dos,
          C_remarks_dos = project.C_remarks_dos,

          D_plastic_curtains_dos = project.D_plastic_curtains_dos,
          D_compliance_dos = project.D_compliance_dos,
          D_remarks_dos = project.D_remarks_dos,

          E_no_accessories_dos = project.E_no_accessories_dos,
          E_compliance_dos = project.E_compliance_dos,
          E_remarks_dos = project.E_remarks_dos,

          F_no_pests_sightings_dos = project.F_no_pests_sightings_dos,
          F_remarks_dos = project.F_remarks_dos,
          F_compliance_dos = project.F_compliance_dos,
          //SECTION 3
          //A
          A_pallet_crates_tres = project.A_pallet_crates_tres,
          A_compliance_tres = project.A_compliance_tres,
          A_remarks_tres = project.A_remarks_tres,

          B_product_contamination_tres = project.B_product_contamination_tres,
          B_compliance_tres = project.B_compliance_tres,
          B_remarks_tres = project.B_remarks_tres,

          C_uncessary_items_tres = project.C_uncessary_items_tres,
          C_compliance_tres = project.C_compliance_tres,
          C_remarks_tres = project.C_remarks_tres,

          D_products_cover_tres = project.D_products_cover_tres,
          D_compliance_tres = project.D_compliance_tres,
          D_remarks_tres = project.D_remarks_tres,
          //SECTION 4
          //A
          A_certificate_coa_kwatro_desc = project.A_certificate_coa_kwatro_desc,
          A_compliance_kwatro = project.A_compliance_kwatro,
          A_remarks_kwatro = project.A_remarks_kwatro,
          //B
          B_po_kwatro_desc = project.B_po_kwatro_desc,
          B_compliance_kwatro = project.B_compliance_kwatro,
          B_remarks_kwatro = project.B_remarks_kwatro,
          //C
          C_msds_kwatro_desc = project.C_msds_kwatro_desc,
          C_compliance_kwatro = project.C_compliance_kwatro,
          C_remarks_kwatro = project.C_remarks_kwatro,
          //D
          D_food_grade_desc = project.D_food_grade_desc,
          D_compliance_kwatro = project.D_compliance_kwatro,
          D_remarks_kwatro = project.D_remarks_kwatro,
          //SECTION 5
          //A

          A_qty_received_singko_singko = project.A_qty_received_singko_singko,
          A_compliance_singko = project.A_compliance_singko,
          A_remarks_singko = project.A_remarks_singko,
          //B
          B_mfg_date_desc_singko = project.B_mfg_date_desc_singko,
          B_compliance_singko = project.B_compliance_singko,
          B_remarks_singko = project.B_remarks_singko,
          //C
          C_expirydate_desc_singko = project.C_expirydate_desc_singko,
          C_compliance_singko = project.C_compliance_singko,
          C_remarks_singko = project.C_remarks_singko,
          //D
          D_packaging_desc_singko = project.D_packaging_desc_singko,
          D_compliance_singko = project.D_compliance_singko,
          D_remarks_singko = project.D_remarks_singko,
          //E
          E_no_contaminants_desc_singko = project.E_no_contaminants_desc_singko,
          E_compliance_singko = project.E_compliance_singko,
          E_remarks_singko = project.E_remarks_singko,
          //F
          F_qtyrejected_desc_singko = project.F_qtyrejected_desc_singko,
          F_compliance_singko = project.F_compliance_singko,
          F_remarks_singko = project.F_remarks_singko,
          //G
          G_rejected_reason_desc_singko = project.G_rejected_reason_desc_singko,
          G_compliance_singko = project.G_compliance_singko,
          G_remarks_singko = project.G_remarks_singko,
          //H
          H_lab_sample_desc_singko = project.H_lab_sample_desc_singko,
          H_compliance_singko = project.H_compliance_singko,
          H_remarks_singko = project.H_remarks_singko,

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



      //projects = db.Projects.Include("ClientLocation").Where(temp => temp.Po_number.Contains(searchText)).ToList();
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


    //Nearly Expiry Approval
    [HttpPut]
    [Route("api/ProjectsPartialPo")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Put([FromBody] Project project)
    {
      RMProjectsPartialPo existingProject = db.ProjectsPartialPo.Where(temp => temp.ProjectID == project.ProjectID).FirstOrDefault();
      if (existingProject != null)
      {
   

        //Aproval opf the 
        existingProject.Is_approved_XP = project.Is_approved_XP;
        existingProject.Is_approved_by = project.Is_approved_by;
        existingProject.Is_approved_date = project.Is_approved_date;

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






  }
}
