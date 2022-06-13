using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class ParentCheckList
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
    //public ICollection<ChildCheckList> ChildCheckLists { get; set; }
    public ParentCheckList()
    {
      ChildCheckLists = new HashSet<ChildCheckList>();
      GrandChildCheckLists = new HashSet<GrandChildCheckList>();
      CheckListParameters = new HashSet<CheckListParameters>();
    }
    public virtual ICollection<ChildCheckList> ChildCheckLists { get; set; }
    public virtual ICollection<GrandChildCheckList> GrandChildCheckLists { get; set; }
    public virtual ICollection<CheckListParameters> CheckListParameters{ get; set; }
  }
}
