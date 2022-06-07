using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class ChildCheckListViewModel
  {
    public int Cc_id { get; set; }
    public string Cc_parent_key { get; set; }
    public string Cc_description { get; set; }
    public string Cc_bool_status { get; set; }
    public string Cc_added_by { get; set; }
    public string Cc_date_added { get; set; }
    public bool Is_active { get; set; }
    public string Updated_at { get; set; }
    public string Updated_by { get; set; }
    public string Deactivated_by { get; set; }
    public string Deactivated_at { get; set; }


  }
}
