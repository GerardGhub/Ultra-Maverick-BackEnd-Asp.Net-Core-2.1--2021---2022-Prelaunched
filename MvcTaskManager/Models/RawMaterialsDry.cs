using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class RawMaterialsDry
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int item_id { get; set; }
    public string item_code { get; set; }
    public string item_description { get; set; }
    public string item_class { get; set; }
    public int major_category { get; set; }
    public string sub_category { get; set; }
    public string primary_unit { get; set; }
    public string conversion { get; set; }
    public string item_type { get; set; }
    public bool is_active { get; set; }

  }
}
