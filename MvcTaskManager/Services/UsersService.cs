using MvcTaskManager.Identity;
using MvcTaskManager.ServiceContracts;
using MvcTaskManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MvcTaskManager.Models;

namespace MvcTaskManager.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly ApplicationSignInManager _applicationSignInManager;
        private readonly ApplicationDbContext _db;

        public UsersService(ApplicationUserManager applicationUserManager, ApplicationSignInManager applicationSignInManager, IOptions<AppSettings> appSettings, ApplicationDbContext db)
        {
            this._applicationUserManager = applicationUserManager;
            this._applicationSignInManager = applicationSignInManager;
            this._appSettings = appSettings.Value;
            this._db = db;
        }

        public async Task<ApplicationUser> Authenticate(LoginViewModel loginViewModel)
        {
            var result = await _applicationSignInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);
            if (result.Succeeded)
            {
                var applicationUser = await _applicationUserManager.FindByNameAsync(loginViewModel.Username);
                applicationUser.PasswordHash = null;
                if (await this._applicationUserManager.IsInRoleAsync(applicationUser, "Admin")) applicationUser.Role = "Admin";
                else if (await this._applicationUserManager.IsInRoleAsync(applicationUser, "Employee")) applicationUser.Role = "Employee";

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, applicationUser.Id),
                        new Claim(ClaimTypes.Email, applicationUser.Email),
                        new Claim(ClaimTypes.Role, applicationUser.Role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(8),
                    SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                applicationUser.Token = tokenHandler.WriteToken(token);

                return applicationUser;
            }
            else
            {
                return null;
            }
        }

        public async Task<ApplicationUser> Register(SignUpViewModel signUpViewModel)
        {

          ApplicationUser applicationUser = new ApplicationUser();
          applicationUser.FirstName = signUpViewModel.FirstName;
          applicationUser.LastName = signUpViewModel.LastName;


          applicationUser.Gender = signUpViewModel.Gender;
          applicationUser.Role = "Admin";
          applicationUser.UserName = signUpViewModel.UserName;
          applicationUser.Email = signUpViewModel.UserName;
          applicationUser.UserRole = signUpViewModel.UserRole;

          applicationUser.Department_id = signUpViewModel.Department_id;
 
          applicationUser.Unit_id = signUpViewModel.Unit_id;

          applicationUser.Location = signUpViewModel.Location;

          applicationUser.First_approver_name = signUpViewModel.First_approver_name;
          applicationUser.First_approver_id = (int)signUpViewModel.First_approver_id;
          applicationUser.Second_approver_name = signUpViewModel.Second_approver_name;
          applicationUser.Second_approver_id = (int)signUpViewModel.Second_approver_id;
          applicationUser.Third_approver_name = signUpViewModel.Third_approver_name;
          applicationUser.Third_approver_id = (int)signUpViewModel.Third_approver_id;
          applicationUser.Fourth_approver_name = signUpViewModel.Fourth_approver_name;
          applicationUser.Fourth_approver_id = (int)signUpViewModel.Fourth_approver_id;
          applicationUser.Approver = signUpViewModel.Approver;
          applicationUser.Requestor = signUpViewModel.Requestor;
          applicationUser.Is_active = true;
          applicationUser.Employee_number = signUpViewModel.Employee_number;
          applicationUser.EncryptPassword = signUpViewModel.Password;


      var TotalUsers = await _db.Users.Where(temp => temp.LockoutEnabled.Equals(true)).ToListAsync();
   
      applicationUser.User_Identity = signUpViewModel.User_Identity = TotalUsers.Count + 1;

      var result = await _applicationUserManager.CreateAsync(applicationUser, signUpViewModel.Password);
            if (result.Succeeded)
            {
                if ((await _applicationUserManager.AddToRoleAsync(await _applicationUserManager.FindByNameAsync(signUpViewModel.UserName), "Admin")).Succeeded)
                {
                    var result2 = await _applicationSignInManager.PasswordSignInAsync(signUpViewModel.UserName, signUpViewModel.Password, false, false);
                    if (result2.Succeeded)
                    {
                        //token
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);
                        var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
                        {
                            Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, applicationUser.Id),
                        new Claim(ClaimTypes.Email, applicationUser.Email),
                        new Claim(ClaimTypes.Role, applicationUser.Role)
                    }),
                            Expires = DateTime.UtcNow.AddHours(8),
                            SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        applicationUser.Token = tokenHandler.WriteToken(token);

      
           await  this._db.SaveChangesAsync();

            return applicationUser;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<ApplicationUser> GetUserByEmail(string Email)
        {
            return await _applicationUserManager.FindByEmailAsync(Email);
        }
    }
}


