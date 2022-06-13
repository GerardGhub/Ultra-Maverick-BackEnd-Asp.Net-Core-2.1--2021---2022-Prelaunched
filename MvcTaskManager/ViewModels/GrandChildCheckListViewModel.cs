using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class GrandChildCheckListViewModel
  {
    public int Gc_id { get; set; }
    public string Gc_child_key { get; set; }
    public string Gc_bool_status { get; set; }
    public string Gc_added_by { get; set; }
    public string Gc_date_added { get; set; }
    public bool Is_active { get; set; }
    public string Updated_at { get; set; }
    public string Updated_by { get; set; }
    public string Deactivated_by { get; set; }
    public string Deactivated_at { get; set; }
    public string Gc_description { get; set; }
    public string Is_manual { get; set; }
    public int Parent_chck_id_fk { get; set; }
    public int Parent_chck_id { get; set; }
  }
}
