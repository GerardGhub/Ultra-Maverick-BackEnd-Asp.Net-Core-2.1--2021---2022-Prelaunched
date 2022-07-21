using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class DryWhOrder
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int primary_id { get; set; }
    public string is_approved_prepa_date { get; set; }
    public string store_name { get; set; }
    public string route { get; set; }
    public string area { get; set; }
    public string category { get; set; }
    public bool is_active { get; set; }
    public string is_for_validation { get; set; }
    public string is_approved { get; set; }
    public string is_prepared { get; set; }
    public string force_prepared_status { get; set; }
    public string fox { get; set; }
    public string item_code { get; set; }

    public string description { get; set; }
    public string uom { get; set; }

    public string total_state_repack { get; set; }
    public string qty { get; set; }

    public string prepared_allocated_qty { get; set; }
    public string is_wh_approved { get; set; }
    public string is_wh_approved_by { get; set; }
    public string is_wh_approved_date { get; set; }


    public string is_wh_checker_cancel { get; set; }
    public string is_wh_checker_cancel_by { get; set; }
    public string is_wh_checker_cancel_date { get; set; }
    public string is_wh_checker_cancel_reason { get; set; }

    public string dispossal_status { get; set; }
    public int? wh_checker_move_order_no { get; set; }
    public int? total_state_repack_cancelled_qty { get; set; }
    public string logic_return_by { get; set; }
    public string logic_return_date { get; set; }
    public string logic_return_reason { get; set; }

  }
}
