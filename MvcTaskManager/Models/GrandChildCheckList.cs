using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class GrandChildCheckList
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int gc_id { get; set; }
    public string gc_child_key { get; set; }
    public string gc_child_po_number { get; set; }
    public string gc_bool_status { get; set; }
    public string gc_added_by { get; set; }
    public string gc_date_added { get; set; } = DateTime.Now.ToString();
    public bool is_active { get; set; } = true;
    public string updated_at { get; set; }
    public string updated_by { get; set; }
    public string deactivated_by { get; set; }
    public string deactivated_at { get; set; }
    public string gc_description { get; set; }

  }
}
