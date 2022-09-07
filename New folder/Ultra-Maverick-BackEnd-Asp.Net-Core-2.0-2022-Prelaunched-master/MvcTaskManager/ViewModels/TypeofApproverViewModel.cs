using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class TypeofApproverViewModel
  {
    public int Approver_id { get; set; }
    public string Type_of_approver { get; set; }
    public string Created_at { get; set; }
    public string Created_by { get; set; }
    public string Updated_at { get; set; }
    public string Updated_by { get; set; }
    public bool Is_active { get; set; }

  }
}
