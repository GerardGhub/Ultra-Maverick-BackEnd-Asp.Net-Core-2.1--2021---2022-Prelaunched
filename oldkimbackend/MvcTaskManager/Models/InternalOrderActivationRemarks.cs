using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class InternalOrderActivationRemarks
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int soar_id { get; set; }
    public string soar_desc { get; set; }
    public string soar_type { get; set; }
    public string soar_added_by { get; set; }
    public string soar_date_added { get; set; }
    public bool soar_is_active { get; set; }
    public string soar_updated_by { get; set; }
    public string soar_updated_date { get; set; }

  }
}
