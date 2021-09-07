using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shatably.BLL.Authentication;
using Shatably.BLL.Model;
using Shatably.Data.Context;
using Shatably.Data.Models;

namespace Shatably.BLL
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ShatblyDbContext shatblyDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountRepository(ShatblyDbContext shatblyDbContext, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.shatblyDbContext = shatblyDbContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> AddAdmin(RegisterModel User)
        {
            var Newuser = new ApplicationUser
            {
                UserName = User.UserName,
                Email = User.EmailAddress,
                FirstName = User.FirstName,
                LastName = User.LastName,
                BirthDate = User.BirthDate,
                Gender = User.Gender,
                MobileNumber = User.MobileNumber,
                UserType = "Admin"
            };

            var Result = await userManager.CreateAsync(Newuser, User.Password);

            var roleExist = await roleManager.RoleExistsAsync(UserRoles.Admin);

            if (!roleExist)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = UserRoles.Admin
                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult resultRole = await roleManager.CreateAsync(identityRole);

                if (!resultRole.Succeeded)
                    return resultRole;
            }

            var AddedUSerToRole = await userManager.AddToRoleAsync(Newuser, UserRoles.Admin);

            if (!AddedUSerToRole.Succeeded)
                return AddedUSerToRole;

            return Result;
        }

        public async Task<OperationResult> AddCompanyUser(RegisterModel User)
        {
            var Newuser = new ApplicationUser
            {
                UserName = User.UserName,//Company Name
                Email = User.EmailAddress,
                FirstName = User.FirstName,
                LastName = User.LastName,
                BirthDate = User.BirthDate,
                Gender = User.Gender,
                MobileNumber = User.MobileNumber,
                ContactRole = User.ContactRole.ToString(),
                UserType = "Company"
            };

            var Result = await userManager.CreateAsync(Newuser, User.Password);

            if (!Result.Succeeded)
                return OperationResult.FailureResult(Result);

            var roleExist = await roleManager.RoleExistsAsync(UserRoles.Company);

            if (!roleExist)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = UserRoles.Company
                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult resultRole = await roleManager.CreateAsync(identityRole);

                if (!resultRole.Succeeded)
                    return OperationResult.FailureResult(resultRole);
            }

            var AddedUSerToRole = await userManager.AddToRoleAsync(Newuser, UserRoles.Company);

            if (!AddedUSerToRole.Succeeded)
                return OperationResult.FailureResult(AddedUSerToRole);

            return OperationResult.SuccessResultAdd(Newuser, Result);
        }

        public async Task<OperationResult> AddUser(RegisterModel User)
        {
            var Newuser = new ApplicationUser
            {
                UserName = User.EmailAddress,
                Email = User.EmailAddress,
                FirstName = User.FirstName,
                LastName = User.LastName,
                BirthDate = User.BirthDate,
                Gender = User.Gender,
                MobileNumber = User.MobileNumber,
                UserType = "User"
            };

            var Result = await userManager.CreateAsync(Newuser, User.Password);

            if (!Result.Succeeded)
                return OperationResult.FailureResult(Result);

            var roleExist = await roleManager.RoleExistsAsync(UserRoles.User);

            if (!roleExist)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = UserRoles.User
                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult resultRole = await roleManager.CreateAsync(identityRole);

                if (!resultRole.Succeeded)
                    return OperationResult.FailureResult(resultRole);
            }

            var AddedUSerToRole = await userManager.AddToRoleAsync(Newuser, UserRoles.User);

            if (!AddedUSerToRole.Succeeded)
                return OperationResult.FailureResult(AddedUSerToRole);


            return OperationResult.SuccessResultAdd(Newuser, Result);
        }

        public async Task<string> CreateEmailConfirmationToken(ApplicationUser User)
        {
            var EmailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(User);

            return EmailConfirmationToken;
        }

        public async Task<IdentityResult> ConfirmEmail(ApplicationUser user, string token)
        {
            var result = await userManager.ConfirmEmailAsync(user, token);

            return result;
        }

        public async Task<bool> CheckUserPasswrd(ApplicationUser applicationUser, string Password)
        {
            var check = await userManager.CheckPasswordAsync(applicationUser, Password);

            return check;
        }
        public async Task<string> GenerateFogotPasswordToken(ApplicationUser User)
        {
            var FogotPasswordToken = await userManager.GeneratePasswordResetTokenAsync(User); ;

            return FogotPasswordToken;
        }
        public async Task<IdentityResult> ResetPassword(ApplicationUser User, string Password, string token)
        {
            var result = await userManager.ResetPasswordAsync(User, token, Password);

            return result;

        }

        public async Task<IdentityResult> DeleteUser(string UserId)
        {
            var user = await FindById(UserId);

            var result = await userManager.DeleteAsync(user);
            return result;
        }

        public async Task<ApplicationUser> FindByEmail(string UserEmail)
        {
            var data = await userManager.FindByEmailAsync(UserEmail);
            return data;
        }

        public async Task<ApplicationUser> FindById(string Id)
        {
            var data = await userManager.FindByIdAsync(Id);
            return data;
        }

        public async Task<ApplicationUser> FindByMobileNumber(int MobileNumber)
        {
            var data = await userManager.Users.FirstOrDefaultAsync(a => a.MobileNumber == MobileNumber);
            return data;
        }

        public async Task<ApplicationUser> FindByName(string UserName)
        {
            var data = await userManager.FindByNameAsync(UserName);
            return data;
        }

        public async Task<IEnumerable<ApplicationUser>> FindByType(string Type)
        {
            var data = await userManager.Users.Where(a => a.UserType == Type).ToListAsync();
            return data;
        }

        public IQueryable<ApplicationUser> GetAllUsers()
        {
            var ListUsers = userManager.Users;
            return ListUsers;
        }

        public async Task<IEnumerable<string>> GetUserRole(ApplicationUser User)
        {
            var data = await userManager.GetRolesAsync(User);
            return data;
        }

        public async Task<SignInResult> SignInUser(string UserName, string Password)
        {
            var result = await signInManager.PasswordSignInAsync(
                  UserName, Password, true, false);

            return result;
        }

        //public async Task<IdentityResult> UpdateCompany(EditModel User)
        //{
        //    var userExist = await FindById(User.Id);
        //    if (userExist != null)
        //    {
        //        userExist.UserName = User.UserName;
        //        userExist.Gender = User.Gender;
        //        userExist.MobileNumber = User.MobileNumber;
        //        userExist.BirthDate = User.BirthDate;
        //        userExist.FirstName = User.FirstName;
        //        userExist.LastName = User.LastName;
        //        userExist.ContactRole = User.ContactRole.ToString();

        //        var result = await userManager.UpdateAsync(userExist);

        //        return result;
        //    }
        //    return null;
        //}

        public async Task<IdentityResult> UpdateUser(EditModel User)
        {
            var userExist = await FindById(User.Id);

            if (userExist != null)
            {
                userExist.UserName = User.UserName;
                userExist.Gender = User.Gender;
                userExist.MobileNumber = User.MobileNumber;
                userExist.BirthDate = User.BirthDate;
                userExist.FirstName = User.FirstName;
                userExist.LastName = User.LastName;
                userExist.ContactRole = User.ContactRole.ToString();

                var result = await userManager.UpdateAsync(userExist);

                return result;
            }
            return null;
        }

        public async Task<IdentityResult> ChangePassword(ApplicationUser User, string NewPassword, string OldPassword)
        {
            var result = await userManager.ChangePasswordAsync(User, OldPassword, NewPassword);

            await signInManager.RefreshSignInAsync(User);

            return result;
        }
    }
}
