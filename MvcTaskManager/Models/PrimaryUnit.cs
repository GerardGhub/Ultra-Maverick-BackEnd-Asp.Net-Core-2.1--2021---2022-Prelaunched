using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class PrimaryUnit
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int unit_id { get; set; }
    public string unit_desc { get; set; }
    public string pm_added_by { get; set; }
    public string pm_added_at { get; set; }
    public string pm_updated_at { get; set; }
    public string pm_updated_by { get; set; }
    public bool  is_active { get; set; }
  }
}
