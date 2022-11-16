using MvcTaskManager.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class MaterialRequestMaster
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int mrs_id { get; set; }
    public string mrs_req_desc { get; set; }
    public string mrs_requested_date { get; set; } = DateTime.Now.ToString("M/d/yyyy");
    public string mrs_date_needed { get; set; }
    public string mrs_requested_by { get; set; }
    public int department_id { get; set; }
    public string is_cancel_by { get; set; }
    public string is_cancel_reason { get; set; }
    public string is_cancel_date { get; set; }
    public bool is_active { get; set; } = true;
    public string is_approved_by { get; set; }
    public string is_approved_date { get; set; }
    public string updated_by { get; set; }
    public string updated_date { get; set; }
    public bool is_prepared { get; set; }
    public string is_for_validation { get; set; } = "0";
    public bool is_wh_sup_approval { get; set; }

    public string Force_prepared_status { get; set;}
    public string Is_wh_sup_approval_date { get; set; }
    public string Is_wh_preparation_date { get; set; }


    public bool Is_wh_checker_approval { get; set; }
    public string Is_wh_checker_approval_by { get; set; }
    public DateTime? Is_wh_checker_approval_date { get; set; }
    public int Wh_checker_move_order_no { get; set; }

    [Required]
    public int user_id { get; set; }

    


    public MaterialRequestMaster()
    {
      MaterialRequestLogs = new HashSet<MaterialRequestLogs>();

    }
    public ICollection<MaterialRequestLogs> MaterialRequestLogs { get; set; }




  }


}
