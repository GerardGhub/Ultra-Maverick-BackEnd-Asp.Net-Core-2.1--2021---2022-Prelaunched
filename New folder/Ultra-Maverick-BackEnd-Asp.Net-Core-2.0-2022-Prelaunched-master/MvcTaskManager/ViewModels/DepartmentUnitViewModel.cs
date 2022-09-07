using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class DepartmentUnitViewModel
  {
    public int Unit_id { get; set; }
    public string Unit_description { get; set; }
    public string Department { get; set; }
    public string Updated_at { get; set; }
    public string Updated_by { get; set; }
    public string Created_at { get; set; }
    public string Created_by { get; set; }
    public bool Is_active { get; set; }
  }
}
