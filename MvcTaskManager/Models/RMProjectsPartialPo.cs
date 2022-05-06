using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class RMProjectsPartialPo
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ProjectID { get; set; }
    //public int PrimaryID { get; set; }


    public string ProjectName { get; set; }

    [DisplayFormat(DataFormatString = "d/M/yyyy")]
    public DateTime DateOfStart { get; set; }

    public int? TeamSize { get; set; }

    public bool Active { get; set; }

    public string Status { get; set; }
    public string Supplier { get; set; }

    public string is_activated { get; set; }
    public string item_code { get; set; }
    public string item_description { get; set; }
    public string Po_number { get; set; }
    public string Po_date { get; set; }
    public string Pr_number { get; set; }
    public string Pr_date { get; set; }
    public string Qty_order { get; set; }
    public string Qty_uom { get; set; }
    public string Mfg_date { get; set; }
    public string Expiration_date { get; set; }
    public string Expected_delivery { get; set; }
    public string Actual_delivery { get; set; }
    public string Actual_remaining_receiving { get; set; }
    public string Received_by_QA { get; set; }
    public string Status_of_reject_one { get; set; }
    public string Status_of_reject_two { get; set; }
    public string Status_of_reject_three { get; set; }
    public string Count_of_reject_one { get; set; }
    public string Count_of_reject_two { get; set; }
    public string Count_of_reject_three { get; set; }
    public string Total_of_reject_mat { get; set; }
    //SECTION 1
    //A
    public string A_delivery_van_desc { get; set; }
    public string A_compliance { get; set; }
    public string A_remarks { get; set; }
    //B
    public string B_cooling_system_desc { get; set; }
    public string B_compliance { get; set; }
    public string B_remarks { get; set; }
    //C
    public string C_inner_walls_desc { get; set; }
    public string C_compliance { get; set; }
    public string C_remarks { get; set; }
    //D
    public string D_plastic_curtains_desc { get; set; }
    public string D_compliance { get; set; }
    public string D_remarks { get; set; }
    //E
    public string E_thereno_pest_desc { get; set; }
    public string E_compliance { get; set; }
    public string E_remarks { get; set; }
    //SECTION 2
    //A
    public string A_clean_company_dos { get; set; }
    public string A_compliance_dos { get; set; }
    public string A_remarks_dos { get; set; }
    //B
    public string B_delivery_staff_symptoms_dos { get; set; }
    public string B_compliance_dos { get; set; }
    public string B_remarks_dos { get; set; }
    //C
    public string C_inner_walls_clean_dos { get; set; }
    public string C_compliance_dos { get; set; }
    public string C_remarks_dos { get; set; }
    //D
    public string D_plastic_curtains_dos { get; set; }
    public string D_compliance_dos { get; set; }
    public string D_remarks_dos { get; set; }
    //E
    public string E_no_accessories_dos { get; set; }
    public string E_compliance_dos { get; set; }
    public string E_remarks_dos { get; set; }
    //F
    public string F_compliance_dos { get; set; }
    public string F_no_pests_sightings_dos { get; set; }
    public string F_remarks_dos { get; set; }
    //Section 3
    //A
    public string A_pallet_crates_tres { get; set; }
    public string A_compliance_tres { get; set; }
    public string A_remarks_tres { get; set; }
    //B1

    public string B_product_contamination_tres { get; set; }
    public string B_compliance_tres { get; set; }
    public string B_remarks_tres { get; set; }

    //C

    public string C_uncessary_items_tres { get; set; }
    public string C_compliance_tres { get; set; }
    public string C_remarks_tres { get; set; }

    //D
    public string D_products_cover_tres { get; set; }
    public string D_compliance_tres { get; set; }
    public string D_remarks_tres { get; set; }
    //Sectionn 4
    //A
    public string A_certificate_coa_kwatro_desc { get; set; }
    public string A_compliance_kwatro { get; set; }
    public string A_remarks_kwatro { get; set; }
    //B
    public string B_po_kwatro_desc { get; set; }
    public string B_compliance_kwatro { get; set; }
    public string B_remarks_kwatro { get; set; }
    //C
    public string C_msds_kwatro_desc { get; set; }
    public string C_compliance_kwatro { get; set; }
    public string C_remarks_kwatro { get; set; }
    //D
    public string D_food_grade_desc { get; set; }
    public string D_compliance_kwatro { get; set; }
    public string D_remarks_kwatro { get; set; }

    //SECTION 5
    //A 
    public string A_qty_received_singko_singko { get; set; }
    public string A_compliance_singko { get; set; }
    public string A_remarks_singko { get; set; }
    //B
    public string B_mfg_date_desc_singko { get; set; }
    public string B_compliance_singko { get; set; }
    public string B_remarks_singko { get; set; }

    //C
    public string C_expirydate_desc_singko { get; set; }
    public string C_compliance_singko { get; set; }
    public string C_remarks_singko { get; set; }

    //D
    public string D_packaging_desc_singko { get; set; }
    public string D_compliance_singko { get; set; }
    public string D_remarks_singko { get; set; }

    //E
    public string E_no_contaminants_desc_singko { get; set; }
    public string E_compliance_singko { get; set; }
    public string E_remarks_singko { get; set; }
    //F
    public string F_qtyrejected_desc_singko { get; set; }
    public string F_compliance_singko { get; set; }
    public string F_remarks_singko { get; set; }
    //G
    public string G_rejected_reason_desc_singko { get; set; }
    public string G_compliance_singko { get; set; }
    public string G_remarks_singko { get; set; }

    public string H_lab_sample_desc_singko { get; set; }
    public string H_compliance_singko { get; set; }
    public string H_remarks_singko { get; set; }
    //Cancel Model Po transactrion
    public string Cancelled_date { get; set; }
    public string Canceled_by { get; set; }
    public string Cancelled_reason { get; set; }
    //Returned entities
    public string Returned_date { get; set; }
    public string Returned_by { get; set; }
    public string Returned_reason { get; set; }
    public int ClientLocationID { get; set; }
    public string QCReceivingDate { get; set; }
    //Xprity
    public int Days_expiry_setup { get; set; }
    public string Is_expired { get; set; }
    //Aproval
    public string Is_approved_XP { get; set; }
    public string Is_approved_by { get; set; }
    public string Is_approved_date { get; set; }

    //public DateTime Expiration_date_string { get; set; }
    //Warehouse Receiving First entry of rejection Anakputa!
    public string Is_wh_received { get; set; }
    public string Is_wh_received_by { get; set; }
    public string Is_wh_received_date { get; set; }
    public string Is_wh_reject { get; set; }

    //Rejection Approval of QC Supervisor
    public string Is_wh_reject_approval { get; set; }
    public string Is_wh_reject_approval_by { get; set; }
    public string Is_wh_reject_approval_date { get; set; }
    //Rejection Approval Receiving Statusof the Entity
    public string Is_wh_reject_approval_is_received { get; set; }
    public string Is_wh_reject_approval_is_received_date { get; set;}
    public string Is_wh_reject_approval_is_received_by { get; set; }


    [ForeignKey("ClientLocationID")]
    public virtual ClientLocation ClientLocation { get; set; }
  



   
  }
}
