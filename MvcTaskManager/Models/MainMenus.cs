using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace MvcTaskManager.Models
{
  public class MainMenus
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Mainmodulename { get; set; }
    public DateTime DateAdded { get; set; }
    public string AddedBy { get; set; }
    public bool  Isactive { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string Reason { get; set; }
    public string MenuPath { get; set; }
    public string isactivereference { get; set; }
  }
}
