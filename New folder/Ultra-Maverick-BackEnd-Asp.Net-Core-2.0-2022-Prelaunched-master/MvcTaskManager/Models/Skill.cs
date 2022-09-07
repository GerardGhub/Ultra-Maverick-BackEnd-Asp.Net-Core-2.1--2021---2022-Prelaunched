using MvcTaskManager.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
    public class Skill
    {
      [Key]
      [Required]
      public int SkillID { get; set; }
      [Required]
      public string SkillName { get; set; }
      [Required]
      public string SkillLevel { get; set; }
      [Required]
      public string Id { get; set; }

      [ForeignKey("Id")]
      public virtual ApplicationUser ApplicationUser { get; set; }
    }
}


