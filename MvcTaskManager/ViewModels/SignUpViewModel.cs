using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.ViewModels
{
    public class SignUpViewModel
    {
    //[Required]
    //public PersonFullName PersonName { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }


    public string UserName { get; set; }

     public string Password { get; set; }
    [Required]
    public string Gender { get; set; }

    [Required]
    public string UserRole { get; set; }
    [Required]
    public int Employee_number { get; set; }


    //Material Request
    [Required]
    public int Department_id { get; set; }

    [Required]
    public int Unit_id { get; set; }
    [Required]
    public string Location { get; set; }

    public string First_approver_name { get; set; }

    public int? First_approver_id { get; set; }

    public string Second_approver_name { get; set; }

    public int? Second_approver_id { get; set; }

    public string Third_approver_name { get; set; }

    public int? Third_approver_id { get; set; }

    public string Fourth_approver_name { get; set; }

    public int? Fourth_approver_id { get; set; }
    //public bool Is_active { get; set; }
    public bool Approver { get; set; }
    public bool Requestor { get; set; }
    //public string EncryptPassword { get; set; }
    public int User_Identity { get; set; }

    //public MaterialRequestRequirement MaterialRequest{ get; set; }
  }


  public class MaterialRequestRequirement
  {
    [Required]
    public int Department_id { get; set; }

    [Required]
    public int Unit_id { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
 
 
    public string First_approver_name { get; set; }

    public int? First_approver_id { get; set; }

    public string Second_approver_name { get; set; }

    public int? Second_approver_id { get; set; }

    public string Third_approver_name { get; set; }

    public int? Third_approver_id { get; set; }

    public string Fourth_approver_name { get; set; }

    public int? Fourth_approver_id { get; set; }
    public bool Is_active { get; set; }
    public bool Approver { get; set; }
    public bool Requestor { get; set; }
  }


  public class PersonFullName
    {
     [Required]
     public string FirstName { get; set; }
     [Required]
    public string LastName { get; set; }
    }
}


