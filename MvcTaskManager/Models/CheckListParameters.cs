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
    public int gc_id { get; set; }
    public string cp_gchild_key { get; set; }
    public string parent_chck_details { get; set; }

    public string cp_added_by { get; set; }
    public string cp_date_added { get; set; } = DateTime.Now.ToString();
    public bool is_active { get; set; } = true;
    public string updated_at { get; set; }
    public string updated_by { get; set; }
    public string deactivated_by { get; set; }
    public string deactivated_at { get; set; }
    public string cp_description { get; set; }
    public int parent_chck_id_fk { get; set; }
    public int parent_chck_id { get; set; }
    public bool cp_status { get; set; }
    public string manual_description { get; set; }

    public CheckListParameters()
    {
      DynamicChecklistLoggers = new HashSet<DynamicChecklistLogger>();

    }
    public ICollection<DynamicChecklistLogger> DynamicChecklistLoggers { get; set; }

    //[ForeignKey("cp_params_id")]
    //public virtual DynamicChecklistLogger DynamicChecklistLogger { get; set; }

  }
}
