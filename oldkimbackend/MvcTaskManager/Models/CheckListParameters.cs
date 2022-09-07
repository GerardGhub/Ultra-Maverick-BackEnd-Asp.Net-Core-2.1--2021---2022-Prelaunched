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
    public int Cp_params_id { get; set; }
    public int Gc_id { get; set; }
    public string Gc_description { get; set; }
    public string Cp_gchild_key { get; set; }
    public string Parent_chck_details { get; set; }

    public string Cp_added_by { get; set; }
    public string Cp_date_added { get; set; } = DateTime.Now.ToString();
    public bool Is_active { get; set; } = true;
    public string Updated_at { get; set; }
    public string Updated_by { get; set; }
    public string Deactivated_by { get; set; }
    public string Deactivated_at { get; set; }
    public string Cp_description { get; set; }
    public int Parent_chck_id_fk { get; set; }
    public int Parent_chck_id { get; set; }
    public bool Cp_status { get; set; }
    public string Manual_description { get; set; }

    public CheckListParameters()
    {
      DynamicChecklistLoggers = new HashSet<DynamicChecklistLogger>();

    }
    public ICollection<DynamicChecklistLogger> DynamicChecklistLoggers { get; set; }

    //[ForeignKey("cp_params_id")]
    //public virtual DynamicChecklistLogger DynamicChecklistLogger { get; set; }

  }
}
