using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcTaskManager.Models
{
  public class Modules
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int Mainmenuid { get; set; }
    public string Submenuname { get; set; }
    public string Modulename { get; set; }
    public DateTime DateAdded { get; set; }
    public bool Isactive { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string Reason { get; set;}
    public string ModuleStatus { get; set; }
    public string AddedBy { get; set; }
    public string isactivereference { get; set; }

    public string Modulestatus { get; set; }
  }
}
