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

    public int ProjectID { get; set; }
    public int parent_id { get; set; }
    public int gc_id { get; set; }
    public int cp_params_id { get; set; }
    public string parent_desc { get; set; }
    public string grand_child_desc { get; set; }
    [Required]
    public string manual_description { get; set; }
    public bool cp_status { get; set; }

    public int parent_chck_id { get; set; }
    public string cp_description { get; set; }
    //public ICollection<ParentCheckList> ParentCheckLists { get; set; }

    //[ForeignKey("parent_id")]
    //public virtual ParentCheckList ParentCheckList { get; set; }

  }
}
