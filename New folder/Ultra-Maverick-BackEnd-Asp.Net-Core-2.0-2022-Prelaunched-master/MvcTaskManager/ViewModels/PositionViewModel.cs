using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class PositionViewModel
  {
    public int Position_id { get; set; }
    public string Position_name { get; set; }
    public string Department_id { get; set; }
    public string Created_by { get; set; }
    public string Created_at { get; set; }
    public string Modified_by { get; set; }
    public string Modified_date { get; set; }
    public bool Is_active { get; set; }
  }
}
