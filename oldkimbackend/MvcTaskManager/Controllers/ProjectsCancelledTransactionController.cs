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
