using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class DryWhOrderParent
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Is_approved_prepa_date { get; set; }
    public string Approved_preparation { get; set; }
    public string Store_name { get; set; }
    public string Route { get; set; }
    public string Area { get; set; }
    public string Category { get; set; }
    public bool Is_active { get; set; }
    public string Is_for_validation { get; set; }
    public bool Is_approved { get; set; }
    public bool Is_prepared { get; set; }
    public string Force_prepared_status { get; set; }
    public string Fox { get; set; }
    public bool Is_wh_approved {get; set;}
    public string Is_wh_approved_by { get; set; }
    public string Is_wh_approved_date { get; set; }
    public string Wh_checker_move_order_no { get; set; }
    public string Is_cancel_by { get; set; }
    public DateTime? Is_cancel_date { get; set; } 

  }
}
