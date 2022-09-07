using Microsoft.AspNetCore.Identity;
using MvcTaskManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Identity
{
  public class ApplicationUserGetter : IdentityUser
  {
    [NotMapped]
    public string Token { get; set; }

    [NotMapped]
    public string Role { get; set; }


    public string FirstName { get; set; }
    public string LastName { get; set; }
    //public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }

    //public int CountryID { get; set; }
    public string UserRole { get; set; }
    //public bool ReceiveNewsLetters { get; set; }

    public int Department_id { get; set; }
    public int Position_id { get; set; }
    public int Unit_id { get; set; }
    public string Location { get; set; }


    public string First_approver_name { get; set; }

    public int? First_approver_id { get; set; }

    public string Second_approver_name { get; set; }

    public int? Second_approver_id { get; set; }

    public string Third_approver_name { get; set; }

    public int? Third_approver_id { get; set; }

    public string Fourth_approver_name { get; set; }

    public int? Fourth_approver_id { get; set; }

    public bool Is_active { get; set; }
    public bool Requestor { get; set; }
    public bool Approver { get; set; }

    [NotMapped]

    [ForeignKey("department_id")]
    public virtual Department Department { get; set; }


    [NotMapped]

    [ForeignKey("position_id")]
    public virtual Position Position { get; set; }

    [NotMapped]
    [ForeignKey("unit_id")]
    public virtual DepartmentUnit DepartmentUnit { get; set; }
  }
}
