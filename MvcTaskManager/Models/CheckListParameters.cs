using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class CheckListParameters
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int cp_params_id { get; set; }
    public string cp_gchild_key { get; set; }
    public string cp_gchild_po_number { get; set; }
    public string cp_bool_status { get; set; }
    public string cp_added_by { get; set; }
    public string cp_date_added { get; set; } = DateTime.Now.ToString();
    public bool is_active { get; set; } = true;
    public string updated_at { get; set; }
    public string updated_by { get; set; }
    public string deactivated_by { get; set; }
    public string deactivated_at { get; set; }
    public string cp_description { get; set; }


  }
}
