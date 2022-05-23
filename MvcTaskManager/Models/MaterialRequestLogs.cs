using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class MaterialRequestLogs
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int mrs_id { get; set; }

    public int mrs_transact_no { get; set; }
    
    public string mrs_item_code { get; set; }
    public string mrs_item_description { get; set; }

    [Required]
    [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid Target Price; Maximum Two Decimal Points.")]
    public decimal mrs_order_qty { get; set; }
    [Required]
    public string mrs_uom { get; set; }
    public string mrs_served_qty { get; set; }
    public string mrs_remarks { get; set; }
    [Required]
    public string mrs_date_needed { get; set; }
    public string mrs_date_requested { get; set; } = DateTime.Now.ToString("M/d/yyyy");

    public string mrs_approved_by { get; set; }
    public string mrs_approved_date { get; set; }
    public string mrs_issued_by { get; set; }
    public string mrs_issued_date { get; set; }
    public string mrs_requested_by { get; set; }
    public bool is_active { get; set; } = true;
    public string deactivated_by { get; set; }
    public string deactivated_date { get; set; }
    public string activated_by { get; set; }
    public string activated_date { get; set; }
    public int static_count { get; set; }
    public int department_id { get; set; }
   
  }
}
