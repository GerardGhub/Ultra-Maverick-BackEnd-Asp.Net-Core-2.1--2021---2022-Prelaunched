using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class DryWhLabTestReqLogs
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Lab_req_id { get; set; }
    public string Item_code { get; set; }
    public string Item_desc { get; set; }
    public string Category { get; set; }
    public string Qty_received { get; set; }
    public string Remaining_qty { get; set; }
    public string Days_to_expired { get; set; }
    public string Lab_status { get; set; }
    public string Historical { get; set; }
    public string Aging { get; set; }
    public string Remarks { get; set; }
    public int Fk_receiving_id { get; set; }
    public bool Is_active { get; set; }
    public string Added_by { get; set; }
    public DateTime Date_added { get; set; }
    public string Qa_approval_by { get; set; }
    public string Qa_approval_status { get; set; }
    public string Qa_approval_date { get; set; }
    public string Lab_result_released_by { get; set; }
    public string Lab_result_released_date { get; set; }
    public string Lab_result_remarks { get; set; }
    public string Lab_sub_remarks { get; set; }
    public string Lab_exp_date_extension { get; set; }
    public string Laboratory_procedure { get; set; }
    public DateTime Lab_request_date { get; set; }
    public string Lab_result_received_by { get; set; }
    public string Lab_result_received_date { get; set; }
    public string Lab_request_by { get; set; }
    public string Is_received_status { get; set; }

    public int Po_number { get; set; }
    public int Pr_number { get; set; }
    public string Po_date { get; set; }
    public string Pr_date { get; set; }

    public string Lab_access_code { get; set; }

    [DisplayFormat(DataFormatString = "MM/dd/yyyy")]
    public DateTime Bbd { get; set; }


    public bool Qa_supervisor_is_approve_status { get; set; }
    public string Qa_supervisor_is_approve_by { get; set; }
    public string Qa_supervisor_is_approve_date { get; set; }


    public bool Qa_supervisor_is_cancelled_status { get; set; }
    public string Qa_supervisor_is_cancelled_by { get; set; }
    public string Qa_supervisor_is_cancelled_date { get; set; }
    public string Qa_supervisor_cancelled_remarks { get; set; }
    public int Sample_Qty { get; set; }

    public bool Tsqa_Approval_Status { get; set; }
    public string Tsqa_Approval_By { get; set; }
    public DateTime? Tsqa_Approval_Date { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }

    [ForeignKey("fk_receiving_id")]
    public virtual DryWareHouseReceiving DryWareHouseReceiving { get; set; }
  }
}
