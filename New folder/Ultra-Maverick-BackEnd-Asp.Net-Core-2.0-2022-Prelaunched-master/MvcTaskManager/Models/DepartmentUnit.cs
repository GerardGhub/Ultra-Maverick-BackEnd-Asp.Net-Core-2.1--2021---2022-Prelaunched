using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class DepartmentUnit
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int unit_id { get; set; }
    public string unit_description { get; set; }
    public string department { get; set; }
    public string updated_at { get; set; }
    public string updated_by { get; set; }
    public string created_at { get; set; }
    public string created_by { get; set; }
    public bool is_active { get; set; }

  }
}
