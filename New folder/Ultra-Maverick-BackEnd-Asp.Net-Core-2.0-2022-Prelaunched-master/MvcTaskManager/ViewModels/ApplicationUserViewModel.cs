using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
  public class ApplicationUserViewModel
  {

  

    public string Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string Username {get; set;}
    public string SecurityStamp { get; set; }
    public string ConcurrencyStamp { get; set; }
      [Required]
    public string Email { get; set; }
    public string NormalizedEmail { get; set; }
    //[Required]
    //public string Mobile { get; set; }
    //[Required]
    //public string DateOfBirth { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Gender { get; set; }
    //[Required]
    //public int CountryID { get; set; }
    //[Required]
    //public bool ReceiveNewsLetters { get; set; }
    [Required]
    public string UserRole { get; set; }
    //[Required]
    //public List<Skill> Skills { get; set; }
    [Required]
    public int Department_id { get; set; }
    [Required]
    public int Position_id { get; set; }
    [Required]
    public int Unit_id { get; set; }
    [Required]
    public string Location { get; set; }

    [Required]
    public string First_approver_name { get; set; }
    [Required]
    public int? First_approver_id { get; set; }
    [Required]
    public string Second_approver_name { get; set; }
    [Required]
    public int? Second_approver_id { get; set; }
    [Required]
    public string Third_approver_name { get; set; }
    [Required]
    public int? Third_approver_id { get; set; }
    [Required]
    public string Fourth_approver_name { get; set; }
    [Required]
    public int? Fourth_approver_id { get; set; }
    public bool Is_active { get; set; }
    public bool Approver { get; set; }
    public bool Requestor { get; set; }
    public string Department_Name { get; set; }
    public string Position_Name { get; set; }
    public string DepartmentUnit_Name { get; set; }

    //[ForeignKey("department_id")]
    //public virtual Department Department { get; set; }

    //[Required]
    //public MaterialRequestRequirements MaterialRequest { get; set; }
  }




}

