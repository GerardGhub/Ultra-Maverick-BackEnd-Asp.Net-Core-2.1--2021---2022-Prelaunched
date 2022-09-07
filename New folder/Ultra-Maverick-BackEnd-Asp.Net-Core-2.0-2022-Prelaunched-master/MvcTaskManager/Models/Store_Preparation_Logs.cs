using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class Store_Preparation_Logs
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Prepa_Id { get; set; }
    public int Prepa_Source_Key { get; set; }
    public string Prepa_Approved_Prepa_Date { get; set; }
    public string Prepa_Item_Code { get; set; }
    public string Prepa_Item_Description { get; set; }
    public string Prepa_Order_Qty { get; set; }
    public string Prepa_Allocated_Qty { get; set; }
    public string Prepa_Date_Added { get; set; }
    public string Prepa_Added_By { get; set; }
    public bool Is_Active { get; set; }
    public int Order_Source_Key { get; set; }
    public bool Data_Refactoring_Status { get; set; }
    public string Category { get; set; }
    public string Fox_Code { get; set; }

  }
}
