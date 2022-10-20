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
    public int MainMenuId { get; set; }
    public string SubMenuName { get; set; }
    public string ModuleName { get; set; }
    public DateTime DateAdded { get; set; }
    public bool IsActive { get; set; }
    public string ModifiedBy { get; set; }
    public string Reason { get; set;}
    public string ModuleStatus { get; set; }
    public string AddedBy { get; set; }


  }
}
