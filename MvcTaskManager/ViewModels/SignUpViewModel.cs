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
    [Required]
    public PersonFullName PersonName { get; set; }
    [Required]
    public string Email { get; set; }
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
  }

    public class PersonFullName
    {
   [Required]
     public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    }
}


