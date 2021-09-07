using Microsoft.AspNetCore.Identity;
using Shatably.BLL.Authentication;
using Shatably.BLL.Model;
using Shatably.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatably.BLL
{
    public interface IAccountRepository
    {
        IQueryable<ApplicationUser> GetAllUsers();
        Task<OperationResult> AddUser(RegisterModel User);
        Task<IdentityResult> AddAdmin(RegisterModel User);
        Task<OperationResult> AddCompanyUser(RegisterModel User);
        Task<IdentityResult> UpdateUser(EditModel User);
        //Task<IdentityResult> UpdateCompany(EditModelCompany User);
        Task<IdentityResult> DeleteUser(string UserId);
        Task<ApplicationUser> FindByName(string UserName);
        Task<ApplicationUser> FindById(string Id);
        Task<ApplicationUser> FindByMobileNumber(int MobileNumber);
        Task<IEnumerable<ApplicationUser>> FindByType(string Type);
        Task<ApplicationUser> FindByEmail(string UserEmail);
        Task<IEnumerable<string>> GetUserRole(ApplicationUser User);
        Task<SignInResult> SignInUser(string UserName, string Password);
        Task<string> CreateEmailConfirmationToken(ApplicationUser user);
        Task<IdentityResult> ConfirmEmail(ApplicationUser user, string token);
        Task<bool> CheckUserPasswrd(ApplicationUser applicationUser, string Password);
        Task<string> GenerateFogotPasswordToken(ApplicationUser User);
        Task<IdentityResult> ResetPassword(ApplicationUser User, string Password, string token);
        Task<IdentityResult> ChangePassword(ApplicationUser User, string NewPassword, string OldPassword);
    }

}
