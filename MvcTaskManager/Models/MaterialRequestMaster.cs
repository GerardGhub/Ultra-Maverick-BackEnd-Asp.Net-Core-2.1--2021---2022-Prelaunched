using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class MaterialRequestMaster
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int mrs_id { get; set; }
    public string mrs_req_desc { get; set; }

  }


}
