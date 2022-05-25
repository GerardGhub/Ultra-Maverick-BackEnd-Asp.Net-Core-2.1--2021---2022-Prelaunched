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


  }
}
