using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcTaskManager.Identity;
using MvcTaskManager.Models;
using MvcTaskManager.ServiceContracts;
using MvcTaskManager.ViewModels;

namespace MvcTaskManager.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IAntiforgery _antiforgery;
        private readonly ApplicationSignInManager _applicationSignInManager;
        private readonly ApplicationDbContext db;
        private readonly ApplicationUserManager applicationUserManager;
        string checkstatistic = "";
     
    public AccountController(IUsersService usersService, ApplicationSignInManager applicationSignManager, IAntiforgery antiforgery, ApplicationDbContext db, ApplicationUserManager applicationUserManager)
        {
            this._usersService = usersService;
            this._applicationSignInManager = applicationSignManager;
            this._antiforgery = antiforgery;
            this.db = db;
            this.applicationUserManager = applicationUserManager;
       
    }

        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]LoginViewModel loginViewModel)
        {
            if (loginViewModel.Username != null || loginViewModel.Password != null)
            {
                var user = await _usersService.Authenticate(loginViewModel);
                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                HttpContext.User = await _applicationSignInManager.CreateUserPrincipalAsync(user);
                var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
                Response.Headers.Add("Access-Control-Expose-Headers", "XSRF-REQUEST-TOKEN");
                Response.Headers.Add("XSRF-REQUEST-TOKEN", tokens.RequestToken);

                return Ok(user);
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
        }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] SignUpViewModel signUpViewModel)
    {
      var EmailValidation = await db.Users.Where(temp => temp.UserName == signUpViewModel.UserName).ToListAsync();

      if (EmailValidation.Count > 0)
      {
        return BadRequest(new { message = "Username Already Taken" });
      }


      var EmployeeNumberValidation = await db.Users.Where(temp => temp.Employee_number == signUpViewModel.Employee_number).ToListAsync();

      if (EmployeeNumberValidation.Count > 0)
      {
        return BadRequest(new { message = "Employee Already Exist" });
      }

      //2nd Boolean
      if (signUpViewModel.Requestor == true)
      {

        if (signUpViewModel.First_approver_id == null)
        {
          this.checkstatistic = "0";

        }
        else
        {
          this.checkstatistic = "1";
        }

        if (signUpViewModel.Second_approver_id == null)
        {
          if (signUpViewModel.Third_approver_id != null)
          {
            this.checkstatistic = "2";
          }

        }

        if (signUpViewModel.Third_approver_id == null)
        {
          if (signUpViewModel.Fourth_approver_id != null)
          {
            this.checkstatistic = "2";
          }

        }


        if (checkstatistic == "2")
        {
          return BadRequest(new { message = "You must select an initial first approver" });
        }


        if (checkstatistic != "1")
        {
          return BadRequest(new { message = "You must select a approver" });
        }
      }




      if (signUpViewModel.First_approver_id == signUpViewModel.Second_approver_id)
      {
      if (signUpViewModel.First_approver_id != null)
      {
      return BadRequest(new { message = "You already tagged a approver on section 1 and 2" });
      }
      }

      else if (signUpViewModel.First_approver_id == signUpViewModel.Third_approver_id)
      {
        if (signUpViewModel.First_approver_id != null)
        {
          return BadRequest(new { message = "You already tagged a approver on section 1 and 3" });
        }
      }

      else if (signUpViewModel.First_approver_id == signUpViewModel.Fourth_approver_id)
      {
        if (signUpViewModel.First_approver_id != null)
        {
          return BadRequest(new { message = "You already tagged a approver on section 1 and 4" });
        }
      }
      else if (signUpViewModel.Second_approver_id == signUpViewModel.Third_approver_id)
      {
        if (signUpViewModel.Second_approver_id != null)
        {
          return BadRequest(new { message = "You already tagged a approver on section 2 and 3" });
        }
      }

      else if (signUpViewModel.Second_approver_id == signUpViewModel.Fourth_approver_id)
      {
        if (signUpViewModel.Second_approver_id != null)
        {
          return BadRequest(new { message = "You already tagged a approver on section 2 and 4" });
        }
      }

      else if (signUpViewModel.Third_approver_id == signUpViewModel.Fourth_approver_id)
      {
        if (signUpViewModel.Third_approver_id != null)
        {
          return BadRequest(new { message = "You already tagged a approver on section 3 and 4" });
        }
      }




      //  Boolean
      
    


        //Validating Null Value
        //1
      if (signUpViewModel.First_approver_id == null)
      {
        signUpViewModel.First_approver_id = 0;
      }
      //2
      if (signUpViewModel.Second_approver_id == null)
      {
        signUpViewModel.Second_approver_id = 0;
      }
      //3
      if (signUpViewModel.Third_approver_id == null)
      {
        signUpViewModel.Third_approver_id = 0;
      }
      //4
      if (signUpViewModel.Fourth_approver_id == null)
      {
        signUpViewModel.Fourth_approver_id = 0;
      }


      var user = await _usersService.Register(signUpViewModel);

      if (user == null)
        return BadRequest(new { message = "Invalid Data" });


            HttpContext.User = await _applicationSignInManager.CreateUserPrincipalAsync(user);
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            Response.Headers.Add("Access-Control-Expose-Headers", "XSRF-REQUEST-TOKEN");
            Response.Headers.Add("XSRF-REQUEST-TOKEN", tokens.RequestToken);

      return Ok(user);
    }




    [HttpGet]
    [Route("api/getUserByEmail/{Email}")]
        public async Task<IActionResult> GetUserByEmail(string Email)
        {
            var user = await _usersService.GetUserByEmail(Email);
            return Ok(user);
        }


    [HttpGet]
    [Route("api/getallemployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            List<ApplicationUser> users = await this.db.Users.ToListAsync();
            List<ApplicationUser> employeeUsers = new List<ApplicationUser>();

            foreach (var item in users)
            {
                if ((await this.applicationUserManager.IsInRoleAsync(item, "Employee")))
                {
                    employeeUsers.Add(item);
                }
            }
            return Ok(employeeUsers);
        }




    [HttpGet]
    [Route("api/umwebusers_mrs_requestor")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetMrsRequestor()
    {


      List<ApplicationUser> obj = new List<ApplicationUser>();

      var userlist = (from a in db.Users
                      join b in db.Department on a.Department_id equals b.department_id
                      //join c in db.Position on a.Position_id equals c.position_name
                      join d in db.DepartmentUnit on a.Unit_id equals d.unit_id
                      where
                      a.Is_active.Equals(true)
                      && a.Requestor.Equals(true)
                      && b.is_active.Equals(true)
                      //&& c.is_active.Equals(true)
                      && d.is_active.Equals(true)
                      select new
                      {
                        Id = a.Id,
                        Employee_number = a.Employee_number,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        Gender = a.Gender,
                        UserRole = a.UserRole,                      
                        Username = a.UserName,
                        //Email = a.Email,
                        Password = a.PasswordHash,
                        EncryptPassword = a.EncryptPassword,
                        //SecurityStamp = a.SecurityStamp,
                        //ConcurrencyStamp = a.ConcurrencyStamp,
                        Is_active = a.Is_active,
                        Department_id = a.Department_id,
                        //Position_id = a.Position_id,
                        Unit_id = a.Unit_id,
                        Location = a.Location,
                        Approver = a.Approver,
                        Requestor = a.Requestor,
                        First_approver_id = a.First_approver_id,
                        First_approver_name = a.First_approver_name,
                        Second_approver_id = a.Second_approver_id,
                        Second_approver_name = a.Second_approver_name,
                        Third_approver_id = a.Third_approver_id,
                        Third_approver_name = a.Third_approver_name,
                        Fourth_approver_id = a.Fourth_approver_id,
                        Fourth_approver_name = a.Fourth_approver_name,
                        Department_Name = b.department_name,
                        //Position_Name = c.position_name,
                        DepartmentUnit_Name = d.unit_description,
                      


                      });
      foreach (var user in userlist)
      {

        ApplicationUser clr = new ApplicationUser();
        clr.FirstName = user.FirstName;
        clr.LastName = user.LastName;


        obj.Add(clr);




      }

      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(userlist);
      });

      return (result);
      
    
    }







    [HttpGet]
    [Route("api/umwebusers_mrs_approver")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetMrsApprover()
    {
      


      List<ApplicationUser> obj = new List<ApplicationUser>();

      var userlist = (from a in db.Users
                      join b in db.Department on a.Department_id equals b.department_id
                      //join c in db.Position on a.Position_id equals c.position_name
                      join d in db.DepartmentUnit on a.Unit_id equals d.unit_id
                      where
                      a.Is_active.Equals(true)
                      && a.Approver.Equals(true)
                      && b.is_active.Equals(true)
                      //&& c.is_active.Equals(true)
                      && d.is_active.Equals(true)
                      select new
                      {
                        Id = a.Id,
                        Employee_number = a.Employee_number,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        Gender = a.Gender,
                        UserRole = a.UserRole,                 
                        Username = a.UserName,
                        Password = a.PasswordHash,
                        EncryptPassword = a.EncryptPassword,
                        //SecurityStamp = a.SecurityStamp,
                        //ConcurrencyStamp = a.ConcurrencyStamp,
                        Is_active = a.Is_active,
                        Department_id = a.Department_id,
                        //Position_id = a.Position_id,
                        Unit_id = a.Unit_id,
                        Location = a.Location,
                        Approver = a.Approver,
                        Requestor = a.Requestor,
                        First_approver_id = a.First_approver_id,
                        First_approver_name = a.First_approver_name,
                        Second_approver_id = a.Second_approver_id,
                        Second_approver_name = a.Second_approver_name,
                        Third_approver_id = a.Third_approver_id,
                        Third_approver_name = a.Third_approver_name,
                        Fourth_approver_id = a.Fourth_approver_id,
                        Fourth_approver_name = a.Fourth_approver_name,
                        Department_Name = b.department_name,
                        //Position_Name = c.position_name,
                        DepartmentUnit_Name = d.unit_description
            


                      });
      foreach (var user in userlist)
      {

        ApplicationUser clr = new ApplicationUser();
        clr.FirstName = user.FirstName;
        clr.LastName = user.LastName;


        obj.Add(clr);




      }
      //return Ok(userlist);
      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(userlist);
      });

      return (result);


    }

    //ApplicationUser
    [HttpPut]
    [Route("api/umwebusers_update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ApplicationUser> Put([FromBody] ApplicationUser labProc)
    {

    

      ApplicationUser existingDataStatus = await db.Users.Where(temp => temp.Id == labProc.Id).FirstOrDefaultAsync();



      //2nd Boolean
      if (labProc.Requestor == true)
      {

        if (labProc.First_approver_id == null )
        {
          this.checkstatistic = "0";

        }
        else
        {
          this.checkstatistic = "1";
        }

        if (labProc.Second_approver_id == null)
        {
          if (labProc.Third_approver_id != null)
          {
            this.checkstatistic = "2";
          }

        }

        if (labProc.Third_approver_id == null)
        {
          if (labProc.Fourth_approver_id != null)
          {
            this.checkstatistic = "2";
          }

        }


        if (checkstatistic == "2")
        {
          return null;
          //return BadRequest(new { message = "You must select an initial first approver" });
        }


        if (checkstatistic != "1")
        {
          return null;
          //return BadRequest(new { message = "You must select a approver" });
        }
      }




      if (labProc.First_approver_id == labProc.Second_approver_id)
      {
        if (labProc.First_approver_id != null && labProc.Second_approver_id != null)
        {
       
          return null;
     
        }
      }

      else if (labProc.First_approver_id == labProc.Third_approver_id)
      {
        if (labProc.First_approver_id != null && labProc.Third_approver_id != null)
        {
          return null;
          
        }
      }

      else if (labProc.First_approver_id == labProc.Fourth_approver_id)
      {
        if (labProc.First_approver_id != null && labProc.Fourth_approver_id != null)
        {
          return null;
      
        }
      }
      else if (labProc.Second_approver_id == labProc.Third_approver_id)
      {
        if (labProc.Second_approver_id != null && labProc.Third_approver_id != null)
        {
          return null;

        }
      }

      else if (labProc.Second_approver_id == labProc.Fourth_approver_id)
      {
        if (labProc.Second_approver_id != null && labProc.Fourth_approver_id != null)
        {
          return null;
  
        }
      }

      else if (labProc.Third_approver_id == labProc.Fourth_approver_id)
      {
        if (labProc.Third_approver_id != null && labProc.Fourth_approver_id != null)
        {
          return null;
 
        }
      }


      //Validating Null Value
      //1
      if (labProc.First_approver_id == null)
      {
        labProc.First_approver_id = 0;
      }
      //2
      if (labProc.Second_approver_id == null)
      {
        labProc.Second_approver_id = 0;
      }
      //3
      if (labProc.Third_approver_id == null)
      {
        labProc.Third_approver_id = 0;
      }
      //4
      if (labProc.Fourth_approver_id == null)
      {
        labProc.Fourth_approver_id = 0;
      }


      // Excemption attempting to Save on Database
      ApplicationUser applicationUser = new ApplicationUser();
      if (existingDataStatus != null)
        {

        
      existingDataStatus.First_approver_name = labProc.First_approver_name;
      existingDataStatus.First_approver_id = labProc.First_approver_id;
      existingDataStatus.Second_approver_name = labProc.Second_approver_name;
      existingDataStatus.Second_approver_id = labProc.Second_approver_id;
      existingDataStatus.Third_approver_name = labProc.Third_approver_name;
      existingDataStatus.Third_approver_id = labProc.Third_approver_id;
      existingDataStatus.Fourth_approver_name = labProc.Fourth_approver_name;
      existingDataStatus.Fourth_approver_id = labProc.Fourth_approver_id;
      //existingDataStatus.EncryptPassword = labProc.EncryptPassword;
      existingDataStatus.Location = labProc.Location;
      existingDataStatus.UserRole = labProc.UserRole;
       
        await db.SaveChangesAsync();
          return existingDataStatus;
        }
        else
        {
          return null;
        }
      //}

    }



    [HttpPut]
    [Route("api/umwebusers_deactivate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ApplicationUser> PutDeactivation([FromBody] ApplicationUser labProc)
    {
      ApplicationUser existingDataStatus = await db.Users.Where(temp => temp.Id == labProc.Id).FirstOrDefaultAsync();

        if (existingDataStatus != null)
        {
        existingDataStatus.Is_active = false;
  
          await db.SaveChangesAsync();
          return existingDataStatus;
        }
        else
        {
          return null;
        }
    }







    [HttpPut]
    [Route("api/umwebusers_activate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ApplicationUser> PutActivation([FromBody] ApplicationUser labProc)
    {
      ApplicationUser existingDataStatus = await db.Users.Where(temp => temp.Id == labProc.Id).FirstOrDefaultAsync();

      if (existingDataStatus != null)
      {
        existingDataStatus.Is_active = true;

        await db.SaveChangesAsync();
        return existingDataStatus;
      }
      else
      {
        return null;
      }
    }



    [HttpGet]
    [Route("api/umwebusers")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get()
    {
   

      List<ApplicationUser> obj = new List<ApplicationUser>();

      var userlist = (from a in db.Users
                       join b in db.Department on a.Department_id equals b.department_id
                       //join c in db.Position on a.Position_id equals c.position_name
                       join d in db.DepartmentUnit on a.Unit_id equals d.unit_id
                       where
                       a.Is_active.Equals(true)
                       && b.is_active.Equals(true)
                       //&& c.is_active.Equals(true)
                       && d.is_active.Equals(true)
                       orderby a.Date_added descending
                       select new 
                       {
                         Employee_number = a.Employee_number,
                         FirstName = a.FirstName,
                         LastName = a.LastName,
                         Gender = a.Gender,
                         UserRole = a.UserRole,
                         Id = a.Id,
                         Username = a.UserName,
                         Email = a.Email,
                         Password = a.PasswordHash,
                         EncryptPassword = a.EncryptPassword,
                         SecurityStamp = a.SecurityStamp,
                         ConcurrencyStamp = a.ConcurrencyStamp,
                         Is_active = a.Is_active,
                         Department_id = a.Department_id,
                         //Position_id = a.Position_id,
                         Unit_id = a.Unit_id,
                         Location = a.Location,
                         Approver = a.Approver,
                         Requestor = a.Requestor,
                         First_approver_id = a.First_approver_id,
                         First_approver_name = a.First_approver_name,
                         Second_approver_id = a.Second_approver_id,
                         Second_approver_name = a.Second_approver_name,
                         Third_approver_id = a.Third_approver_id,
                         Third_approver_name = a.Third_approver_name,
                         Fourth_approver_id = a.Fourth_approver_id,
                         Fourth_approver_name = a.Fourth_approver_name,
                         Department_Name = b.department_name,
                         //Position_Name = c.position_name,
                         DepartmentUnit_Name = d.unit_description,
                         Date_added = a.Date_added


                       });
      foreach (var user in userlist)
      {

        ApplicationUser clr = new ApplicationUser();
        clr.FirstName = user.FirstName;
        clr.LastName = user.LastName;


        obj.Add(clr);

        


      }
      //return Ok(userlist);
      var result = await System.Threading.Tasks.Task.Run(() =>
      {
        return Ok(userlist);
      });

      return (result);

    }


    //End

  }
}


