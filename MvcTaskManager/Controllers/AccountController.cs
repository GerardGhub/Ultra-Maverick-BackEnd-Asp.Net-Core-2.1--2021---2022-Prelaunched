using System;
using System.Collections.Generic;
using System.Linq;
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
      var EmailValidation = db.Users.Where(temp => temp.Email == signUpViewModel.Email).ToList();

      if (EmailValidation.Count > 0)
      {
        return BadRequest(new { message = "Username Already Taken" });
      }


      //2nd Boolean
      if (signUpViewModel.MaterialRequest.Requestor == true)
      {

        if (signUpViewModel.MaterialRequest.First_approver_id == null)
        {
          this.checkstatistic = "0";

        }
        else
        {
          this.checkstatistic = "1";
        }

        if (signUpViewModel.MaterialRequest.Second_approver_id == null)
        {
          if (signUpViewModel.MaterialRequest.Third_approver_id != null)
          {
            this.checkstatistic = "2";
          }

        }

        if (signUpViewModel.MaterialRequest.Third_approver_id == null)
        {
          if (signUpViewModel.MaterialRequest.Fourth_approver_id != null)
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




      if (signUpViewModel.MaterialRequest.First_approver_id == signUpViewModel.MaterialRequest.Second_approver_id)
      {
      if (signUpViewModel.MaterialRequest.First_approver_id != null)
      {
      return BadRequest(new { message = "You already tagged a approver on section 1 and 2" });
      }
      }

      else if (signUpViewModel.MaterialRequest.First_approver_id == signUpViewModel.MaterialRequest.Third_approver_id)
      {
        if (signUpViewModel.MaterialRequest.First_approver_id != null)
        {
          return BadRequest(new { message = "You already tagged a approver on section 1 and 3" });
        }
      }

      else if (signUpViewModel.MaterialRequest.First_approver_id == signUpViewModel.MaterialRequest.Fourth_approver_id)
      {
        if (signUpViewModel.MaterialRequest.First_approver_id != null)
        {
          return BadRequest(new { message = "You already tagged a approver on section 1 and 4" });
        }
      }
      else if (signUpViewModel.MaterialRequest.Second_approver_id == signUpViewModel.MaterialRequest.Third_approver_id)
      {
        if (signUpViewModel.MaterialRequest.Second_approver_id != null)
        {
          return BadRequest(new { message = "You already tagged a approver on section 2 and 3" });
        }
      }

      else if (signUpViewModel.MaterialRequest.Second_approver_id == signUpViewModel.MaterialRequest.Fourth_approver_id)
      {
        if (signUpViewModel.MaterialRequest.Second_approver_id != null)
        {
          return BadRequest(new { message = "You already tagged a approver on section 2 and 4" });
        }
      }

      else if (signUpViewModel.MaterialRequest.Third_approver_id == signUpViewModel.MaterialRequest.Fourth_approver_id)
      {
        if (signUpViewModel.MaterialRequest.Third_approver_id != null)
        {
          return BadRequest(new { message = "You already tagged a approver on section 3 and 4" });
        }
      }




      //  Boolean
      
    


        //Validating Null Value
        //1
      if (signUpViewModel.MaterialRequest.First_approver_id == null)
      {
        signUpViewModel.MaterialRequest.First_approver_id = 0;
      }
      //2
      if (signUpViewModel.MaterialRequest.Second_approver_id == null)
      {
        signUpViewModel.MaterialRequest.Second_approver_id = 0;
      }
      //3
      if (signUpViewModel.MaterialRequest.Third_approver_id == null)
      {
        signUpViewModel.MaterialRequest.Third_approver_id = 0;
      }
      //4
      if (signUpViewModel.MaterialRequest.Fourth_approver_id == null)
      {
        signUpViewModel.MaterialRequest.Fourth_approver_id = 0;
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


    [HttpPut]
    [Route("updateuser")]
    public async Task<IActionResult> UpdateUserInformation([FromBody] SignUpViewModel signUpViewModel)
    {
      //var EmailValidation = db.Users.Where(temp => temp.Email == signUpViewModel.Email).ToList();

      //if (EmailValidation.Count > 0)
      //{
      //  return BadRequest(new { message = "Email Already taken" });
      //}


      var user = await _usersService.Register(signUpViewModel);

      if (user == null)
        return BadRequest(new { message = "Invalid Data" });




      HttpContext.User = await _applicationSignInManager.CreateUserPrincipalAsync(user);
      var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
      Response.Headers.Add("Access-Control-Expose-Headers", "XSRF-REQUEST-TOKEN");
      Response.Headers.Add("XSRF-REQUEST-TOKEN", tokens.RequestToken);

      return Ok(user);
    }



    [Route("api/getUserByEmail/{Email}")]
        public async Task<IActionResult> GetUserByEmail(string Email)
        {
            var user = await _usersService.GetUserByEmail(Email);
            return Ok(user);
        }

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
      
    
      List<ApplicationUser> departments = await db.Users.Where(temp => temp.Is_active.Equals(true) && temp.Requestor.Equals(true)).ToListAsync();


      List<ApplicationUserViewModel> departmentViewModel = new List<ApplicationUserViewModel>();
      foreach (var dept in departments)
      {

        departmentViewModel.Add(new ApplicationUserViewModel()
        {
          FirstName = dept.FirstName,
          LastName = dept.LastName,
          DateOfBirth = dept.DateOfBirth.ToString("M/d/yyyy"),
          Gender = dept.Gender,
          UserRole = dept.UserRole,
          ReceiveNewsLetters = dept.ReceiveNewsLetters,
          Id = dept.Id.ToString(),
          Username = dept.UserName,
          Email = dept.Email,
          NormalizedEmail = dept.NormalizedEmail,
          Password = dept.PasswordHash,
          SecurityStamp = dept.SecurityStamp,
          ConcurrencyStamp = dept.ConcurrencyStamp,
          Mobile = dept.PhoneNumber,
          Is_active = dept.Is_active
          

        });
      }
      return Ok(departmentViewModel);



    }







    [HttpGet]
    [Route("api/umwebusers_mrs_approver")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetMrsApprover()
    {
      //List<ApplicationUser> AspNetUsers = await db.Users.ToListAsync();
      //return Ok(AspNetUsers);
     
      List<ApplicationUser> departments = await db.Users.Where(temp => temp.Is_active.Equals(true) && temp.Approver.Equals(true)).ToListAsync();


      List<ApplicationUserViewModel> departmentViewModel = new List<ApplicationUserViewModel>();
      foreach (var dept in departments)
      {

        departmentViewModel.Add(new ApplicationUserViewModel()
        {
          FirstName = dept.FirstName,
          LastName = dept.LastName,
          DateOfBirth = dept.DateOfBirth.ToString("M/d/yyyy"),
          Gender = dept.Gender,
          UserRole = dept.UserRole,
          ReceiveNewsLetters = dept.ReceiveNewsLetters,
          Id = dept.Id.ToString(),
          Username = dept.UserName,
          Email = dept.Email,
          NormalizedEmail = dept.NormalizedEmail,
          Password = dept.PasswordHash,
          SecurityStamp = dept.SecurityStamp,
          ConcurrencyStamp = dept.ConcurrencyStamp,
          Mobile = dept.PhoneNumber,
          Is_active = dept.Is_active


        });
      }
      return Ok(departmentViewModel);



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
          //return BadRequest(new { message = "You already tagged a approver on section 1 and 2" });
        }
      }

      else if (labProc.First_approver_id == labProc.Third_approver_id)
      {
        if (labProc.First_approver_id != null && labProc.Third_approver_id != null)
        {
          return null;
          //return BadRequest(new { message = "You already tagged a approver on section 1 and 3" });
        }
      }

      else if (labProc.First_approver_id == labProc.Fourth_approver_id)
      {
        if (labProc.First_approver_id != null && labProc.Fourth_approver_id != null)
        {
          return null;
          //return BadRequest(new { message = "You already tagged a approver on section 1 and 4" });
        }
      }
      else if (labProc.Second_approver_id == labProc.Third_approver_id)
      {
        if (labProc.Second_approver_id != null && labProc.Third_approver_id != null)
        {
          return null;
          //return BadRequest(new { message = "You already tagged a approver on section 2 and 3" });
        }
      }

      else if (labProc.Second_approver_id == labProc.Fourth_approver_id)
      {
        if (labProc.Second_approver_id != null && labProc.Fourth_approver_id != null)
        {
          return null;
          //return BadRequest(new { message = "You already tagged a approver on section 2 and 4" });
        }
      }

      else if (labProc.Third_approver_id == labProc.Fourth_approver_id)
      {
        if (labProc.Third_approver_id != null && labProc.Fourth_approver_id != null)
        {
          return null;
          //return Ok("ddd");
          //return BadRequest(new { message = "You already tagged a approver on section 3 and 4" });
        }
      }




      //  Boolean




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
      //List<ApplicationUser> AspNetUsers = await db.Users.ToListAsync();
      //return Ok(AspNetUsers);
      List<ApplicationUser> departments = await db.Users.Where(temp => temp.Is_active.Equals(true)).ToListAsync();


      List<ApplicationUserViewModel> departmentViewModel = new List<ApplicationUserViewModel>();
      foreach (var dept in departments)
      {

        departmentViewModel.Add(new ApplicationUserViewModel()
        {
          FirstName = dept.FirstName,
          LastName = dept.LastName,
          DateOfBirth = dept.DateOfBirth.ToString("M/d/yyyy"),
          Gender = dept.Gender,
          UserRole = dept.UserRole,
          ReceiveNewsLetters = dept.ReceiveNewsLetters,
          Id = dept.Id.ToString(),
          Username = dept.UserName,
          Email = dept.Email,
          NormalizedEmail = dept.NormalizedEmail,
          Password = dept.PasswordHash,
          SecurityStamp = dept.SecurityStamp,
          ConcurrencyStamp = dept.ConcurrencyStamp,
          Mobile = dept.PhoneNumber,
          Is_active = dept.Is_active


        });
      }
      return Ok(departmentViewModel);



    }



  }
}


