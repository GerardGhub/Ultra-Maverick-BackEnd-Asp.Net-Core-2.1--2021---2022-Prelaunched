using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class SystemCapabilityStatus
  {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int status_id { get; set; }
    public string current_system_status { get; set; }
    public string added_by { get; set; }
    public string created_at { get; set; }
    public string updated_at { get; set; }
  }
}
