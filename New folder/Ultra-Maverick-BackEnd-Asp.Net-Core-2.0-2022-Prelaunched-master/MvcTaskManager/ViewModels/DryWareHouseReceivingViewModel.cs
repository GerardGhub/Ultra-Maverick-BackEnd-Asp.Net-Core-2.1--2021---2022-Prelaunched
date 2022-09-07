using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class DryWareHouseReceivingViewModel
  {
    public int Id { get; set; }
    public string Lab_access_code { get; set; }
    public int Index_id_partial { get; set; }
    public string Item_code { get; set; }
    public string Item_description { get; set; }
    public string Category { get; set; }
    public string Uom { get; set; }
    public string Qty_received { get; set; }
    public int? Historical_lab_transact_count { get; set; }

    public string Lab_status { get; set; }
    public int? Expiry_days_aging { get; set; }
    public string Client_requestor { get; set; }
    public string Lab_request_date { get; set; }
    public string Lab_request_by { get; set; }
    public int Po_number { get; set; }
    public string Is_active { get; set; }

    public string Qa_approval_status { get; set; }
    public string Qa_approval_by { get; set; }
    [DisplayFormat(DataFormatString = "MM/dd/yyyy")]
    public string Qa_approval_date { get; set; }

    public string Lab_result_released_by { get; set; }
    public string Lab_result_released_date { get; set; }
    public string Lab_result_remarks { get; set; }
    public string Lab_sub_remarks { get; set; }

    [DisplayFormat(DataFormatString = "d/M/yyyy")]
    public string Lab_exp_date_extension { get; set; }
    public int? Lab_approval_aging_days { get; set; }
    public string Laboratory_procedure { get; set; }
    public string Supplier { get; set; }
    public string Po_date { get; set; }
    public int Pr_no { get; set; }
    public string Pr_date { get; set; }


    public string Lab_cancel_by { get; set; }
    public string Lab_cancel_date { get; set; }
    public string Lab_cancel_remarks { get; set; }
    public string Sample_Qty { get; set; }
 

  }
}
