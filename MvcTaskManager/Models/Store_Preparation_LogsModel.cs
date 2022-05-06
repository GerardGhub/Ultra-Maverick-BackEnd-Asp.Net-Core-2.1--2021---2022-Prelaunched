using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class Store_Preparation_LogsModel
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int prepa_id { get; set; }
    public string prepa_source_key { get; set; }
    public string prepa_approved_prepa_date { get; set; }
    public string prepa_item_code { get; set; }
    public string prepa_item_description { get; set; }
    public string prepa_order_qty { get; set; }
    public string prepa_allocated_qty { get; set; }
    public string prepa_date_added { get; set; }
    public string prepa_added_by { get; set; }
    public string is_active { get; set; }
    public string order_source_key { get; set; }

    //public ICollection<DryWhOrder> DryOrderActivities { get; set; }

  }
}
