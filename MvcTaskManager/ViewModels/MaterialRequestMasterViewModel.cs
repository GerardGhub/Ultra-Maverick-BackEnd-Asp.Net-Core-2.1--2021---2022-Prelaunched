using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class MaterialRequestMasterViewModel
  {
    public int Mrs_id { get; set; }
    public string Mrs_req_desc { get; set; }
    public string Mrs_requested_date { get; set; }
    public string Mrs_requested_by { get; set; }
    public int Department_id { get; set; }
    public string Is_cancel_by { get; set; }
    public string Is_cancel_reason { get; set; }
    public string Is_cancel_date { get; set; }
    public bool Is_active { get; set; }
    public string Is_approved_by { get; set; }
    public string Is_approved_date { get; set; }
    public string User_id { get; set; }
    public bool Is_prepared { get; set; }

  }
}
