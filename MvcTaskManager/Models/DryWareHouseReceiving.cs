using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class DryWareHouseReceiving
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public string lab_access_code { get; set; }
    public int index_id_partial { get; set; }
    public string item_code { get; set; }
    public string item_description { get; set; }
    public string category { get; set; }
    public string uom { get; set; }
    public string qty_received { get; set; }
    public int? historical_lab_transact_count { get; set; }

    public string lab_status { get; set; }
    public int? expiry_days_aging { get; set; }
    public string client_requestor { get; set; }
    public string lab_request_date { get; set; }
    public string lab_request_by { get; set; }
    public int po_number { get; set; }
    public int is_active { get; set; }

    public string qa_approval_status { get; set; }
    public string qa_approval_by { get; set; }
    //[DisplayFormat(DataFormatString = "MM/dd/yyyy")]

    public DateTime qa_approval_date { get; set; }

    public string lab_result_released_by { get; set; }
    public string lab_result_released_date { get; set; }
    public string lab_result_remarks { get; set; }
    public string lab_sub_remarks { get; set; }

    //[DataType(DataType.Date)]
    //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{MM/dd/yyyy}")]
    public DateTime lab_exp_date_extension { get; set; }
    public int? lab_approval_aging_days { get; set; }
    public string laboratory_procedure { get; set; }
    public string supplier { get; set; }
    public string po_date { get; set; }
    public int pr_no { get; set; }
    public string pr_date { get; set; }


    public string lab_cancel_by { get; set; }
    public string lab_cancel_date { get; set; }
    public string lab_cancel_remarks { get; set; }


    public int FK_Sub_Category_IsExpirable { get; set; }

    public string lab_exp_date_request { get; set; }
    public decimal Sample_Qty { get; set; }

    [ForeignKey("is_active")]
    public virtual tblNearlyExpiryMgmtModel tblNearlyExpiryMgmtModel { get; set; }


  }
}
