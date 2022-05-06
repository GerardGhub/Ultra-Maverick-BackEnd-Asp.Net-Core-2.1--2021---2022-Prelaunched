using System;
using System.Collections.Generic;
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
    public string Mrs_order_qty { get; set; }
    public string Mrs_uom { get; set; }
    public string Mrs_served_qty { get; set; }
    public string Mrs_remarks { get; set; }
    public string Mrs_date_needed { get; set; }
    public string Mrs_date_requested { get; set; }
    public string Mrs_order_by { get; set; }
    public string Mrs_order_date { get; set; }
    public string Mrs_approved_by { get; set; }
    public string Mrs_approved_date { get; set; }
    public string Mrs_issued_by { get; set; }
    public string Mrs_issued_date { get; set; }
    public string Mrs_requested_by { get; set; }
    public bool Is_active { get; set; }
  }
}
