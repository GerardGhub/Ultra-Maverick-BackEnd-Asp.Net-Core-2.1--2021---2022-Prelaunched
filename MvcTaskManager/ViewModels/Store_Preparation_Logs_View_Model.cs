using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class Store_Preparation_Logs_View_Model
  {
    public int Prepa_id { get; set; }
    public string Prepa_source_key { get; set; }
    public string Prepa_approved_prepa_date { get; set; }
    public string Prepa_item_code { get; set; }
    public string Prepa_item_description { get; set; }
    public string Prepa_order_qty { get; set; }
    public string Prepa_allocated_qty { get; set; }
    public string Prepa_date_added { get; set; }
    public string Prepa_added_by { get; set; }
    public string Is_Active { get; set; }
    public string Order_source_key { get; set; }

  }
}
