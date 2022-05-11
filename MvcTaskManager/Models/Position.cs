using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class Position
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int position_id { get; set; }
    public string position_name { get; set; }
    public string department_id { get; set; }
    public string created_by { get; set; }
    public string created_at { get; set; }
    public string modified_by { get; set; }
    public string modified_date { get; set; }
    public bool is_active { get; set; }
  }
}
