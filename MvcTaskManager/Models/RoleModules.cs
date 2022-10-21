using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcTaskManager.Models
{
  public class RoleModules
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string RoleId { get; set; }
    public int ModuleId { get; set; }
    public bool IsActive { get; set; }

  }
}
