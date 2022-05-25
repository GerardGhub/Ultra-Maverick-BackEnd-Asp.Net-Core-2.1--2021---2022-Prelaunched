using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class ParentCheckListViewModel
  {
    public int Parent_chck_id { get; set; }
    public string Parent_chck_details { get; set; }
    public string Parent_chck_added_by { get; set; }
    public string Parent_chck_date_added { get; set; }
    public bool Is_active { get; set; }

  }
}
