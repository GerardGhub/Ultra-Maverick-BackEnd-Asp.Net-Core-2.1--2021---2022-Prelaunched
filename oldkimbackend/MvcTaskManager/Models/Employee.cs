using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcTaskManager.Models
{
  public class Employee
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int emp_id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string department { get; set; }
    public string sub_unit { get; set; }
    public int dept_id { get; set; }
    public int unit_id { get; set; }
    public bool is_active { get; set; }


 
    [ForeignKey("dept_id")]
    public virtual Department Department { get; set; }


    [ForeignKey("unit_id")]
    public virtual DepartmentUnit DepartmentUnit { get; set; }

  }
}
