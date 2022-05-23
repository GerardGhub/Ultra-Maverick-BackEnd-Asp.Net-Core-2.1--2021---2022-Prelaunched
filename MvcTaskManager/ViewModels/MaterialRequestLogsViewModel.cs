using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class MaterialRequestLogsViewModel
  {
    public int Mrs_id { get; set; }
    public string Mrs_transact_no { get; set; }
    public string Mrs_item_code { get; set; }
    public string Mrs_item_description { get; set; }

    [Required]
    [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid Target Price; Maximum Two Decimal Points.")]
    public decimal Mrs_order_qty { get; set; }
    public string Mrs_uom { get; set; }
    public string Mrs_served_qty { get; set; }
    public string Mrs_remarks { get; set; }
    public string Mrs_date_needed { get; set; }
    public string Mrs_date_requested { get; set; }

    public string Mrs_approved_by { get; set; }
    public string Mrs_approved_date { get; set; }
    public string Mrs_issued_by { get; set; }
    public string Mrs_issued_date { get; set; }
    public string Mrs_requested_by { get; set; }
    public bool Is_active { get; set; }
    public int Department_Id { get; set; }
  }

  public class MaterialRequestDistinctPerTransactions
  {

    public string Mrs_transact_no { get; set; }

    [Required]
    [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid Target Price; Maximum Two Decimal Points.")]
    //public decimal Mrs_order_qty { get; set; }
    public int Static_count { get; set; }

    public string Mrs_date_requested { get; set; }

    public string Mrs_requested_by { get; set; }
    public int Department_Id { get; set; }
    public string Department_name { get; set; }
  }
}
