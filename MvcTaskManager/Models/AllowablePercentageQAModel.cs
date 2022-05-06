using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class AllowablePercentageQAModel
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int p_id { get; set; }
    public string p_allowable_percentage { get; set; }
    public string p_is_active { get; set; }
    public string p_date_added { get; set; }
    public string p_added_by { get; set; }
    public string p_date_modified { get; set; }
    public string p_modified_by { get; set; }

  }
}
