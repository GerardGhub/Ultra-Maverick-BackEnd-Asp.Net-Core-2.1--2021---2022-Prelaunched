using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class DryWareHouseReceivingViewModelNearlyExpiry
  {
    public int Id { get; set; }

    public string Item_code { get; set; }
    public string Item_description { get; set; }

    public string Qty_received { get; set; }



    public int? Expiry_days_aging { get; set; }

    public string Standard_Expiry_Days { get; set; }


    [DisplayFormat(DataFormatString = "d/M/yyyy")]
    public string Lab_exp_date_extension { get; set; }

    public string RemainingQty { get; set; }



  }
}
