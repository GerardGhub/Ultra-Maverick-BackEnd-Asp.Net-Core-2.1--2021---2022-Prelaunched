using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class tblLocation
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int location_id { get; set; }
    public string location_name { get; set; }
    public string created_at { get; set; }
    public string created_by { get; set; }
    public string updated_at { get; set; }
    public string updated_by { get; set; }
    public bool is_active { get; set; }

  }
}
