using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class ChildCheckList
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int cc_id { get; set; }
    public string cc_description { get; set; }
    [Required]
    public int cc_parent_key { get; set; }
    [Required]

    public string parent_chck_details { get; set; }
    public string cc_bool_status { get; set; }
    public int parent_chck_id { get; set; }
    public string cc_added_by { get; set; }
    public string cc_date_added { get; set; } = DateTime.Now.ToString();
    public bool is_active { get; set; } = true;
    public string updated_at { get; set; }
    public string updated_by { get; set; }
    public string deactivated_by { get; set; }
    public string deactivated_at { get; set; }

    public ChildCheckList()
    {
      GrandChildCheckLists = new HashSet<GrandChildCheckList>();

    }
    public ICollection<GrandChildCheckList> GrandChildCheckLists { get; set; }


  }
}
