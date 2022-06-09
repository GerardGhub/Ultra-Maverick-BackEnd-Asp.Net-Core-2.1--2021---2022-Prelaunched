using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class DynamicChecklistLogger
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public string ProjectID { get; set; }
    public string parent_id { get; set; }
    public string child_id { get; set; }
    public string grand_child_id { get; set; }
    public string parameters_id { get; set; }
    public string parent_desc { get; set; }
    public string child_desc { get; set; }
    public string grand_child_desc { get; set; }
    [Required]
    public string parameters_status { get; set; }
  }
}
