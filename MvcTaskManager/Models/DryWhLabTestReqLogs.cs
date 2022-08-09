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
    public int lab_req_id { get; set; }
    public string item_code { get; set; }
    public string item_desc { get; set; }
    public string category { get; set; }
    public string qty_received { get; set; }
    public string remaining_qty { get; set; }
    public string days_to_expired { get; set; }
    public string lab_status { get; set; }
    public string historical { get; set; }
    public string aging { get; set; }
    public string remarks { get; set; }
    public int fk_receiving_id { get; set; }
    public string is_active { get; set; }
    public string added_by { get; set; }
    public DateTime date_added { get; set; }
    public string qa_approval_by { get; set; }
    public string qa_approval_status { get; set; }
    public string qa_approval_date { get; set; }
    public string lab_result_released_by { get; set; }
    public string lab_result_released_date { get; set; }
    public string lab_result_remarks { get; set; }
    public string lab_sub_remarks { get; set; }
    public string lab_exp_date_extension { get; set; }
    public string laboratory_procedure { get; set; }
    public DateTime lab_request_date { get; set; }
    public string lab_result_received_by { get; set; }
    public string lab_result_received_date { get; set; }
    public string lab_request_by { get; set; }
    public string is_received_status { get; set; }

    public int po_number { get; set; }
    public int pr_number { get; set; }
    public string po_date { get; set; }
    public string pr_date { get; set; }

    public string lab_access_code { get; set; }

    [DisplayFormat(DataFormatString = "MM/dd/yyyy")]
    public DateTime bbd { get; set; }


    public bool qa_supervisor_is_approve_status { get; set; }
    public string qa_supervisor_is_approve_by { get; set; }
    public string qa_supervisor_is_approve_date { get; set; }


    public bool qa_supervisor_is_cancelled_status { get; set; }
    public string qa_supervisor_is_cancelled_by { get; set; }
    public string qa_supervisor_is_cancelled_date { get; set; }
    public string qa_supervisor_cancelled_remarks { get; set; }
    public int Sample_Qty { get; set; }

    [ForeignKey("fk_receiving_id")]
    public virtual DryWareHouseReceiving DryWareHouseReceiving { get; set; }
  }
}
