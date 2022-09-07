using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class ParentCheckListReturnEagerLoading
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int parent_chck_id { get; set; }
    [Required]
    public string parent_chck_details { get; set; }
    [Required]
    public string parent_chck_added_by { get; set; }
    public string parent_chck_date_added { get; set; } = DateTime.Now.ToString();
    public bool is_active { get; set; } = true;
    public string updated_at { get; set; }
    public string updated_by { get; set; }
    public string deactivated_at { get; set; }
    public string deactivated_by { get; set; }
 
    public ParentCheckListReturnEagerLoading()
    {
      ChildCheckLists = new HashSet<ChildCheckList>();
;
    }
    public ICollection<ChildCheckList> ChildCheckLists { get; set; }


  }
}
