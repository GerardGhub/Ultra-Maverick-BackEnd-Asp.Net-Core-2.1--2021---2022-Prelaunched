using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class EmployeeViewModel
  {
    public int Id { get; set; }
    public int Emp_id { get; set; }
    public string First_name { get; set; }
    public string Last_name { get; set; }
    public string Department { get; set; }
    public string Sub_unit { get; set; }
    public bool Is_active { get; set; }
    public string Department_id { get; set; }
    public string DepartmentUnit_id { get; set; }
  }
}
