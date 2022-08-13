using Microsoft.AspNetCore.Http;
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
    public int Is_active { get; set; }

    public string Qa_approval_status { get; set; }
    public string Qa_approval_by { get; set; }


    public DateTime Qa_approval_date { get; set; }

    public string Lab_result_released_by { get; set; }
    public string Lab_result_released_date { get; set; }
    public string Lab_result_remarks { get; set; }
    public string Lab_sub_remarks { get; set; }


    public DateTime Lab_exp_date_extension { get; set; }
    public int? Lab_approval_aging_days { get; set; }
    public string Laboratory_procedure { get; set; }
    public string Supplier { get; set; }
    public string Po_date { get; set; }
    public int pr_no { get; set; }
    public string pr_date { get; set; }


    public string Lab_cancel_by { get; set; }
    public string Lab_cancel_date { get; set; }
    public string Lab_cancel_remarks { get; set; }


    public int FK_Sub_Category_IsExpirable { get; set; }

    public string Lab_exp_date_request { get; set; }
    public int Sample_Qty { get; set; }

    public bool Tsqa_Approval_Status { get; set; }
    public string Tsqa_Approval_By { get; set; }
    public DateTime? Tsqa_Approval_Date { get; set; }

    public string LabTest_CancelledReason { get; set; }


    public bool Qa_supervisor_is_approve_status { get; set; }

    public string Qa_supervisor_is_approve_date { get; set; }

    [NotMapped]
    public IFormFile files { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }

    [ForeignKey("is_active")]
    public virtual tblNearlyExpiryMgmtModel TblNearlyExpiryMgmtModel { get; set; }


  }
}
