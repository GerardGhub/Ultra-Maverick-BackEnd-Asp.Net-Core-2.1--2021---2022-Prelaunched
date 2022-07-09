using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class InternalOrderActivationRemarksViewModel
  {
    public int Soar_id { get; set; }
    public string Soar_desc { get; set; }
    public string Soar_type { get; set; }
    public string Soar_added_by { get; set; }
    public string Soar_date_added { get; set; }
    public bool Soar_is_active { get; set; }
    public string Soar_updated_by { get; set; }
    public string Soar_updated_date { get; set; }

  }
}
