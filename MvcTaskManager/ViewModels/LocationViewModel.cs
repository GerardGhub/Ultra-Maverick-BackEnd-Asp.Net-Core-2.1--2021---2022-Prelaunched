using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class LocationViewModel
  {
    
    public int Location_id { get; set; }
    public string Location_name { get; set; }
    public string Created_at { get; set; }
    public string Created_by { get; set; }
    public string Updated_at { get; set; }
    public string Updated_by { get; set; }
    public bool Is_active { get; set; }
  }
}
