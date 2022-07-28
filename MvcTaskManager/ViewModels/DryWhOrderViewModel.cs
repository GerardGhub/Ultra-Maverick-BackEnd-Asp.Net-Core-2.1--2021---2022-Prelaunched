using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class DryWhOrderViewModel
  {
    public int Primary_id { get; set; }
    public string Is_approved_prepa_date { get; set; }
    public string Store_name { get; set; }
    public string Route { get; set; }
    public string Area { get; set; }
    public string Category { get; set; }
    public string Is_active { get; set; }
    public string Is_for_validation { get; set; }
    public string Is_approved { get; set; }
    public string Is_prepared { get; set; }
    public string Force_prepared_status { get; set; }
    public string Fox { get; set; }
    public string Item_code { get; set; }
    public string Description { get; set; }
    public string Uom { get; set; }
    public string Total_state_repack { get; set; }
    public string Qty { get; set; }

    public string Prepared_allocated_qty { get; set; }

    public string Is_wh_approved { get; set; }
    public string Is_wh_approved_by { get; set; }
    public string Is_wh_approved_date { get; set; }

    public string Is_wh_checker_cancel { get; set; }
    public string Is_wh_checker_cancel_by { get; set; }
    public string Is_wh_checker_cancel_date { get; set; }
    public string Is_wh_checker_cancel_reason { get; set; }

    public string Dispossal_status { get; set; }
    public string Wh_checker_move_order_no { get; set; }
    public int? Total_state_repack_cancelled_qty { get; set; }

    public string Logic_return_by { get; set; }
    public string Logic_return_date { get; set; }
    public string Logic_return_reason { get; set; }

  }
}

