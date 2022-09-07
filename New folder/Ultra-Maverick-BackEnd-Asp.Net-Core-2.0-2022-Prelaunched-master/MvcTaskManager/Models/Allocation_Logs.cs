using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class Allocation_Logs
  {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int allocation_id { get; set; }
    public string a_item_code { get; set; }
    public string a_item_desc { get; set; }
    public string allocation_qty { get; set; }
    public string allocate_by { get; set; }
    public string allocate_date { get; set; }
    public int order_key { get; set; }
    public bool is_active { get; set; }
    public int total_row { get; set; }
    public string total_column_qty { get; set; }


  }
}
