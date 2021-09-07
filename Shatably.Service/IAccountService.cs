using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shatably.BLL;
using Shatably.BLL.Authentication;
using Shatably.Data.Models;
using Shatably.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shatably.Service
{
    public interface IAccountService 
    {
        Task<Response<string>> RegisterUser(RegisterModel User);
        Task<Response<string>> RegisterCompany(RegisterModel User);
        Task<Response<string>> RegisterAdmin(RegisterModel User);
        Task<Response<string>> Login(LoginModel User);
       // Task<string> GetUserByEmail(string UserEmail);
       Task<Response<ApplicationUser>> GetUserByEmail(string UserEmail);
        Task<Response<string>> UpdateUser(EditModel User);
        Task<Response<string>> DeleteUser(string User);
        Task<Response<string>> ConfimEmail(string userId, string token);
        Task<Response<string>> ForgotPassword(string Email);
        Task<Response<string>> ResetPassword(ResetPasswordModel resetPasswordModel, string token);
        Task<Response<string>> ChnagePassword(ChangePasswordModel resetPasswordModel, string UserId);
    }
}
