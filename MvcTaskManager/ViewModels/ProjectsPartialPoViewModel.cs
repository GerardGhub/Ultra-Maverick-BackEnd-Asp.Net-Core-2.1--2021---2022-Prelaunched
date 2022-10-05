using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class ProjectsPartialPoViewModel
  {
    public int ProjectID { get; set; }
    public int PrimaryID { get; set; }
    public string ProjectName { get; set; }
    public string DateOfStart { get; set; }
    public int? TeamSize { get; set; }
    public bool Active { get; set; }
    public string Status { get; set; }
    public string Supplier { get; set; }
    public string is_activated { get; set; }
    public string item_code { get; set; }
    public string item_description { get; set; }
    public string Po_date { get; set; }
    public string Po_number { get; set; }
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
    public string Unit_price { get; set; }
    public string Status_of_reject_one { get; set; }
    public string Status_of_reject_two { get; set; }
    public string Status_of_reject_three { get; set; }
    public string Count_of_reject_one { get; set; }
    public string Count_of_reject_two { get; set; }
    public string Count_of_reject_three { get; set; }

    public string Total_of_reject_mat { get; set; }
    //SECTION 1
    //A

    //Cancel Model Po transactrion
    public string Cancelled_date { get; set; }
    public string Canceled_by { get; set; }
    public string Cancelled_reason { get; set; }

    //Returned entities
    public string Returned_date { get; set; }
    public string Returned_by { get; set; }
    public string Returned_reason { get; set; }

    public string QCReceivingDate { get; set; }

    public int Days_expiry_setup { get; set; }
    public string Is_expired { get; set; }


    //Approval
    public string Is_approved_XP { get; set; }
    public string Is_approved_by { get; set; }
    public string Is_approved_date { get; set; }
    //public DateTime Expiration_date_string { get; set; }
    //public int DaysBeforeExpired { get; set; }
    // Left Join RM
    public string Item_class { get; set; }
    public string Item_type { get; set; }
    public string Major_category { get; set; }
    public string Sub_category { get; set; }
    public string Is_expirable { get; set; }
    public string Is_wh_received { get; set; }
    public string Is_wh_received_by { get; set; }
    public string Is_wh_received_date { get; set; }
    public string Is_wh_reject { get; set; }

    //Rejection Approval of QC Supervisor
    public string Is_wh_reject_approval { get; set; }
    public string Is_wh_reject_approval_by { get; set; }
    public string Is_wh_reject_approval_date { get; set; }

    public int ClientLocationID { get; set; }
    public ClientLocation ClientLocation { get; set; }


  }
}
