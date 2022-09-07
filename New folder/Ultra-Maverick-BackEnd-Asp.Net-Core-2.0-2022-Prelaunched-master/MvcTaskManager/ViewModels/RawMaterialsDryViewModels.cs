using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class RawMaterialsDryViewModels
  {
    public int Item_id { get; set; }
    public string Item_code { get; set; }
    public string Item_description { get; set; }
    public string Item_class { get; set; }
    public string Major_category { get; set; }
    public string Sub_category { get; set; }
    public string Primary_unit { get; set; }
    public string Conversion { get; set; }
    public string Item_type { get; set; }
    public bool Is_active { get; set; }



  }
}
