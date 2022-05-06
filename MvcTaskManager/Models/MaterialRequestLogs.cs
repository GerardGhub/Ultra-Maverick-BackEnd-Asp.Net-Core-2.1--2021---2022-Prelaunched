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
    public string mrs_transact_no { get; set; }
    public string mrs_item_code { get; set; }
    public string mrs_item_description { get; set; }
    public string mrs_order_qty { get; set; }
    public string mrs_uom { get; set; }
    public string mrs_served_qty { get; set; }
    public string mrs_remarks { get; set; }
    public string mrs_date_needed { get; set; }
    public string mrs_date_requested { get; set; }
    public string mrs_order_by { get; set; }
    public string mrs_order_date { get; set; }
    public string mrs_approved_by { get; set; }
    public string mrs_approved_date { get; set; }
    public string mrs_issued_by { get; set; }
    public string mrs_issued_date { get; set; }
    public string mrs_requested_by { get; set; }
    public bool is_active { get; set; }

  }
}
