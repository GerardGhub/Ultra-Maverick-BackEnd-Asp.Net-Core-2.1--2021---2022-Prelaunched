using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class ParentCheckListViewModel
  {
    public ParentCheckListViewModel()
    {
      Books = new HashSet<ChildCheckList>();
      //Users = new HashSet<User>();
    }
    public int Parent_chck_id { get; set; }
    [Required]
    public string Parent_chck_details { get; set; }
    [Required]
    public string Parent_chck_added_by { get; set; }
    public string Parent_chck_date_added { get; set; }
    public bool Is_active { get; set; }
    //public ICollection<ChildCheckList> ChildCheckLists { get; set; }


    public virtual ICollection<ChildCheckList> Books { get; set; }
    //public virtual ICollection<User> Users { get; set; }

  }
}
