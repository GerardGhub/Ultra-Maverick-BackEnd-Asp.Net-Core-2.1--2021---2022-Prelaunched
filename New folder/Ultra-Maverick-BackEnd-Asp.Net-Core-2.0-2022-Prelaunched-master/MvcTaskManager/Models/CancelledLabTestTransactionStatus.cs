using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class CancelledLabTestTransactionStatus
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Status_Name { get; set; }
    public bool Is_Active { get; set; }
    public string Created_At { get; set; }
    public string Created_By { get; set; }
    public string Updated_At { get; set; }
    public string Updated_By { get; set; }

  }
}
