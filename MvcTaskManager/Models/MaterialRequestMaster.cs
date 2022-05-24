using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class MaterialRequestMaster
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int mrs_id { get; set; }
    public string mrs_req_desc { get; set; }
    public string mrs_requested_date { get; set; } = DateTime.Now.ToString("M/d/yyyy");
    public string mrs_requested_by { get; set; }
    public int department_id { get; set; }
    public string is_cancel_by { get; set; }
    public string is_cancel_reason { get; set; }
    public string is_cancel_date { get; set; }
    public bool is_active { get; set; } = true;

  }


}