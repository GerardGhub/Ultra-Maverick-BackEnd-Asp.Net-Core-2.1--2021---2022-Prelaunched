using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class CheckListParametersViewModel
  {
    public int Cp_params_id { get; set; }
    public int Gc_id { get; set; }
    public string Cp_gchild_key { get; set; }
    public string Cp_gchild_po_number { get; set; }
    public string Cp_bool_status { get; set; }
    public string Cp_added_by { get; set; }
    public string Cp_date_added { get; set; }
    public bool Is_active { get; set; }
    public string Updated_at { get; set; }
    public string Updated_by { get; set; }
    public string Deactivated_by { get; set; }
    public string Deactivated_at { get; set; }
    public string Cp_description { get; set; }
    public int Parent_chck_id_fk { get; set; }
    public int Parent_chck_id { get; set; }

  }
}
