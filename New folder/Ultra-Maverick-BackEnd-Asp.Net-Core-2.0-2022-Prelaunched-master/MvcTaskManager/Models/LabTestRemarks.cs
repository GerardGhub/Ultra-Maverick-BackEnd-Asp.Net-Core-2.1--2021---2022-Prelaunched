using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class LabTestRemarks
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int lab_remarks_id { get; set; }
    public string lab_remarks_description { get; set; }
    public string lab_test_remarks_active_status { get; set; }
    public string created_at { get; set; }
    public string created_by { get; set; }
    public string updated_at { get; set; }
    public string updated_by { get; set; }

  }
}
