using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class WarehouseRejectStatusViewModel
  {
    public int Id { get; set; }
    public int Index_id { get; set; }
    public int Po_number { get; set; }
    public int Qty_reject { get; set; }
    public string Reject_remarks { get; set; }
    public string Added_by { get; set; }
    public string Is_pending { get; set; }
    public string Is_active { get; set; }

  }
}
