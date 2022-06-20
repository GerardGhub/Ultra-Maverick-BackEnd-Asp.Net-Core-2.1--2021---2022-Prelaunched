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
    public string parameters_status { get; set; }

    ////New Form of Data
    //public DynamicChecklistLogger()
    //{
    //  CheckListParameters = new HashSet<CheckListParameters>();
    //}
    //public ICollection<CheckListParameters> CheckListParameters { get; set; }

  }
}
