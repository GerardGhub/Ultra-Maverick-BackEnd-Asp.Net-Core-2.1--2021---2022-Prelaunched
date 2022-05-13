using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Required]
    public string Mobile { get; set; }
    [Required]
    public string DateOfBirth { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Gender { get; set; }
    [Required]
    public int CountryID { get; set; }
    [Required]
    public bool ReceiveNewsLetters { get; set; }
    [Required]
    public string UserRole { get; set; }
    [Required]
    public List<Skill> Skills { get; set; }
    [Required]
    public int Department_id { get; set; }
    [Required]
    public int Position_id { get; set; }
    [Required]
    public int Unit_id { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public string Type_of_approver { get; set; }
    [Required]
    public string First_approver_name { get; set; }
    [Required]
    public int First_approver_id { get; set; }
    [Required]
    public string Second_approver_name { get; set; }
    [Required]
    public int Second_approver_id { get; set; }
    [Required]
    public string Third_approver_name { get; set; }
    [Required]
    public int Third_approver_id { get; set; }
    [Required]
    public string Fourth_approver_name { get; set; }
    [Required]
    public int Fourth_approver_id { get; set; }
    public bool Is_active { get; set; }
    //[Required]
    //public MaterialRequestRequirements MaterialRequest { get; set; }
  }


  //public class MaterialRequestRequirements
  //{
  //  [Required]
  //  public int Department_id { get; set; }
  //  [Required]
  //  public int Position_id { get; set; }
  //  [Required]
  //  public int Unit_id { get; set; }
  //  [Required]
  //  public string Location { get; set; }
  //  [Required]
  //  public string Type_of_approver { get; set; }
  //  [Required]
  //  public string First_approver_name { get; set; }
  //  [Required]
  //  public int First_approver_id { get; set; }
  //  [Required]
  //  public string Second_approver_name { get; set; }
  //  [Required]
  //  public int Second_approver_id { get; set; }
  //  [Required]
  //  public string Third_approver_name { get; set; }
  //  [Required]
  //  public int Third_approver_id { get; set; }
  //  [Required]
  //  public string Fourth_approver_name { get; set; }
  //  [Required]
  //  public int Fourth_approver_id { get; set; }
  //  public bool Is_active { get; set; }
  //}


  //public class PersonFullName
  //{
  //  [Required]
  //  public string FirstName { get; set; }
  //  [Required]
  //  public string LastName { get; set; }
  //}


}

