using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shatably.BLL;
using Shatably.BLL.Authentication;
using Shatably.Service.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shatably.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Shatably.Service.Email;
using System.Text.Json;
using Shatably.Service.ViewModel;
using System.Web;
using System.Net;
using Microsoft.AspNetCore.DataProtection;

namespace Shatably.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IConfiguration config;
        private readonly IEmailSender emailSender;
        private readonly IDataProtectionProvider dataProtectionProvider;
        private readonly IDataProtector PdataProtector;

        public AccountService(IAccountRepository accountRepository, IConfiguration config,
            IEmailSender emailSender, IDataProtectionProvider dataProtectionProvider)
        {
            this.accountRepository = accountRepository;
            this.config = config;
            this.emailSender = emailSender;
            this.dataProtectionProvider = dataProtectionProvider;
            PdataProtector = dataProtectionProvider.CreateProtector("TokenValue");
        }

        public async Task<Response<string>> Login(LoginModel User)
        {
            string ErrorMsg = "";

            var UserData = await accountRepository.FindByEmail(User.Email);

            if (UserData != null && UserData.EmailConfirmed
                && await accountRepository.CheckUserPasswrd(UserData, User.Password))
            {
                var userRole = await accountRepository.GetUserRole(UserData);

                if (userRole.Count() > 0)
                {
                    var tokenString = GenerateJSONWebToken(userRole, UserData);

                    var SignIn = await accountRepository.SignInUser(UserData.UserName, User.Password);

                    if (!SignIn.Succeeded)
                    {
                        ErrorMsg = "Invalid Login and/or password";
                        return new Response<string>(ErrorMsg);
                    }

                    return new Response<string>(tokenString, "Successfully created token");
                    // return OperationResult.SuccessResultLogin(tokenString, "Successfully created token");
                }

                ErrorMsg = "Could not Create Token";

                return new Response<string>(ErrorMsg);

            }
            ErrorMsg = "Email User Does not Exist";

            return new Response<string>(ErrorMsg);

        }

        private string GenerateJSONWebToken(IEnumerable<string> UserRole, ApplicationUser User)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, User.FirstName + " " + User.LastName),
                new Claim(ClaimTypes.Email, User.Email),
                new Claim(ClaimTypes.Role, UserRole.FirstOrDefault())
            };
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]));

            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: config["Jwt:ValidIssuer"],
              audience: config["Jwt:ValidAudience"],
              expires: DateTime.Now.AddMinutes(120),
              claims: authClaims,
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Response<string>> RegisterCompany(RegisterModel User)
        {
            string ErrorMsg = "";

            var UserNameExsits = await accountRepository.FindByName(User.UserName);

            if (UserNameExsits != null)
            {
                ErrorMsg = "Username already in use";

                return new Response<string>(ErrorMsg);
            }

            var EmailExsits = await accountRepository.FindByEmail(User.EmailAddress);

            if (EmailExsits != null)
            {
                ErrorMsg = "Email already in use";

                return new Response<string>(ErrorMsg);
            }

            var MobileExsits = await accountRepository.FindByMobileNumber(User.MobileNumber);

            if (MobileExsits != null)
            {
                ErrorMsg = "Mobile already in use";

                return new Response<string>(ErrorMsg);
            }

            var result = await accountRepository.AddCompanyUser(User);

            if (!result.IdentityResult.Succeeded)
            {
                foreach (var item in result.IdentityResult.Errors)
                {
                    ErrorMsg = ErrorMsg + " " + item.Description;
                }
                //  ErrorMsg = "Error adding user";

                return new Response<string>(ErrorMsg);
            }

            var EmailToken = await accountRepository.CreateEmailConfirmationToken(result.applicationUser);

            string codeHtmlVersion = HttpUtility.UrlEncode(EmailToken);
            //codeHtmlVersion = Encoding.

            codeHtmlVersion = codeHtmlVersion.Replace("+", "\n$&");

            var confirmationLinkMsg = @"http://localhost:60639/api/Account/ConfirmEmail?UserId=" + result.applicationUser.Id + "&token=" + codeHtmlVersion;

            var Message = new Message(new string[] { User.EmailAddress }, "Shatably.com"
                , ConfirmationEmailMsgDesign.EmailMsgDesign(User.EmailAddress, confirmationLinkMsg));

            //   await emailSender.SendEmailAsync(Message);

            //var EmailConfirmation = await accountRepository.EmailConfirmation()

            return new Response<string>(true, "Company Registered Successfully");
        }

        public async Task<Response<string>> RegisterUser(RegisterModel User)
        {
            string ErrorMsg = "";

            var EmailExsits = await accountRepository.FindByEmail(User.EmailAddress);

            if (EmailExsits != null)
            {
                ErrorMsg = "Email already in use";

                return new Response<string>(ErrorMsg);
            }

            var MobileExsits = await accountRepository.FindByMobileNumber(User.MobileNumber);

            if (MobileExsits != null)
            {
                ErrorMsg = "Mobile already in use";

                return new Response<string>(ErrorMsg);
            }

            var result = await accountRepository.AddUser(User);

            if (!result.IdentityResult.Succeeded)
            {
                foreach (var item in result.IdentityResult.Errors)
                {
                    ErrorMsg = ErrorMsg + " " + item.Description;
                }

                return new Response<string>(ErrorMsg);
            }

            var EmailToken = await accountRepository.CreateEmailConfirmationToken(result.applicationUser);

            //string codeHtmlVersion = WebUtility.UrlEncode(EmailToken);

            //codeHtmlVersion = WebUtility.UrlEncode(codeHtmlVersion);
            ////codeHtmlVersion = Encoding.

            //codeHtmlVersion = codeHtmlVersion.Replace("+", "$$$$$$");

            string tokenProtectr = PdataProtector.Protect(EmailToken);

            var confirmationLinkMsg = @"http://localhost:60639/api/Account/ConfirmEmail?UserId=" + result.applicationUser.Id + "&token=" + tokenProtectr;

            var Message = new Message(new string[] { User.EmailAddress }, "Shatably.com"
                , ConfirmationEmailMsgDesign.EmailMsgDesign(User.EmailAddress, confirmationLinkMsg));

            await emailSender.SendEmailAsync(Message);

            //var EmailConfirmation = await accountRepository.EmailConfirmation()
            return new Response<string>(true, "User Registerd Successfully");
        }

        public async Task<Response<string>> UpdateUser(EditModel UpdatedUser)
        {
            string ErrorMsg = "";

            var UserExsits = await accountRepository.FindById(UpdatedUser.Id);

            if (UserExsits == null)
            {
                ErrorMsg = "User Doesnot Exist";

                return new Response<string>(ErrorMsg);
            }

            ApplicationUser User = new ApplicationUser
            {
                Id = UpdatedUser.Id,
                UserName = UpdatedUser.UserName,
                Gender = UpdatedUser.Gender,
                MobileNumber = UpdatedUser.MobileNumber,
                BirthDate = UpdatedUser.BirthDate,
                FirstName = UpdatedUser.FirstName,
                LastName = UpdatedUser.LastName,
                ContactRole = UpdatedUser.ContactRole.ToString()
            };

            var result = await accountRepository.UpdateUser(UpdatedUser);

            if (!result.Succeeded)
            {
                ErrorMsg = "Error adding user";

                return new Response<string>(ErrorMsg);
            }
            return new Response<string>(true, "User Updates Successfully");
        }

        public async Task<Response<string>> DeleteUser(string Id)
        {
            string ErrorMsg = "";

            var UserExsits = await accountRepository.FindById(Id);

            if (UserExsits == null)
            {
                ErrorMsg = "User Doesnot Exist";

                return new Response<string>(ErrorMsg);
            }

            var result = await accountRepository.DeleteUser(Id);

            if (!result.Succeeded)
            {
                ErrorMsg = "Error adding user";

                return new Response<string>(ErrorMsg);
            }
            return new Response<string>(true, " User Deleted Successfully!");

        }

        public async Task<Response<ApplicationUser>> GetUserByEmail(string UserEmail)
        {
            var data = await accountRepository.FindByEmail(UserEmail);

            string ErrorMsg = "";

            if (data == null)
            {
                ErrorMsg = "Invalide Email";
                return new Response<ApplicationUser>(ErrorMsg);
            }
            else
            {
                return new Response<ApplicationUser>(data, "Successfully");
            }

        }

        public async Task<Response<string>> RegisterAdmin(RegisterModel User)
        {
            string ErrorMsg = "";

            var UserNameExsits = await accountRepository.FindByName(User.UserName);

            if (UserNameExsits != null)
            {
                ErrorMsg = "Username already in use";

                return new Response<string>(ErrorMsg);
            }

            var EmailExsits = await accountRepository.FindByEmail(User.EmailAddress);

            if (EmailExsits != null)
            {
                ErrorMsg = "Email already in use";

                return new Response<string>(ErrorMsg);
            }

            var MobileExsits = await accountRepository.FindByMobileNumber(User.MobileNumber);

            if (MobileExsits != null)
            {
                ErrorMsg = "Mobile already in use";

                return new Response<string>(ErrorMsg);
            }

            var result = await accountRepository.AddAdmin(User);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ErrorMsg = ErrorMsg + " " + item.Description;
                }
                //  ErrorMsg = "Error adding user";

                return new Response<string>(ErrorMsg);
            }
            return new Response<string>(true, "Company Registered Successfully");
        }

        public async Task<Response<string>> ConfimEmail(string userId, string token)
        {
            string ErrorMsg = "";

            if (userId == null || token == null)
            {
                ErrorMsg = "Invalid Url";
                return new Response<string>(ErrorMsg);
            }

            var user = await accountRepository.FindById(userId);
            if (user == null)
            {
                ErrorMsg = $"The User ID {userId} is invalid";
                return new Response<string>(ErrorMsg);
            }

            // token =  token.Replace("$$$$$$", "+");
            //string decodeHtmlVersion = WebUtility.UrlDecode(token);


            string tokenUnProtectr = PdataProtector.Unprotect(token);

            var data = await accountRepository.ConfirmEmail(user, tokenUnProtectr);

            if (!data.Succeeded)
            {
                foreach (var item in data.Errors)
                {
                    ErrorMsg = ErrorMsg + " " + item.Description;
                }

                return new Response<string>(ErrorMsg);
            }

            return new Response<string>(true, "Email Confirmed");
        }

        public async Task<Response<string>> ForgotPassword(string Email)
        {
            string ErrorMsg = "";

            if (Email == null)
            {
                ErrorMsg = "Email is Empty";
                return new Response<string>(ErrorMsg);
            }

            //We shouldn't say that email is exist for hackers
            var user = await accountRepository.FindByEmail(Email);

            if (user != null && user.EmailConfirmed)
            {
                var token = await accountRepository.GenerateFogotPasswordToken(user);

                if (string.IsNullOrEmpty(token))
                {
                    ErrorMsg = "could not generate token";
                    return new Response<string>(ErrorMsg);
                }
                //string codeHtmlVersion = HttpUtility.UrlEncode(token);

                //codeHtmlVersion = codeHtmlVersion.Replace("+", "\n$&");

                string tokenProtectr = PdataProtector.Protect(token);

                var confirmationLinkMsg = @"http://localhost:60639/api/Account/ResetPassword?Token=" + tokenProtectr;

                var Message = new Message(new string[] { user.Email }, "Shatably.com"
                    , ConfirmationEmailMsgDesign.EmailMsgDesign(user.Email, confirmationLinkMsg));

                await emailSender.SendEmailAsync(Message);

                return new Response<string>(true, "Email sent");
            }

            ErrorMsg = "Email User Does not Exist";

            return new Response<string>(ErrorMsg);

        }

        public async Task<Response<string>> ResetPassword(ResetPasswordModel resetPasswordModel, string token)
        {
            string ErrorMsg = "";

            var user = await accountRepository.FindByEmail(resetPasswordModel.Email);

            if (user != null)
            {

                //string tokenreplaced = token.Replace("\n$&", "+");
                //string decodeHtmlVersion = HttpUtility.UrlDecode(tokenreplaced);

                string tokenUnProtectr = PdataProtector.Unprotect(token);

                var result = await accountRepository.ResetPassword(user, resetPasswordModel.Password, tokenUnProtectr);


                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ErrorMsg = ErrorMsg + " " + item.Description;
                    }

                    return new Response<string>(ErrorMsg);
                }
                return new Response<string>(true, "Passwrod Reset Successfully");
            }

            ErrorMsg = "User Does not Exist";

            return new Response<string>(ErrorMsg);

        }

        public async Task<Response<string>> ChnagePassword(ChangePasswordModel changePasswordModel, string UserId)
        {
            string ErrorMsg = "";

            var user = await accountRepository.FindById(UserId);

            if (user != null)
            {
                var result = await accountRepository.ChangePassword(user, changePasswordModel.NewPassword, changePasswordModel.CurrentPassword);


                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ErrorMsg = ErrorMsg + " " + item.Description;
                    }

                    return new Response<string>(ErrorMsg);
                }
                return new Response<string>(true, "Passwrod Changed Successfully");
            }

            ErrorMsg = "User Does not Exist";

            return new Response<string>(ErrorMsg);
        }

        //public async Task<string> GetUserByEmail(string UserEmail)
        //{
        //    try
        //    {
        //        var userViewModel = new GetUserViewModel();
        //        string ErrorMsg = "";

        //        var data = await accountRepository.FindByEmail(UserEmail);
        //        string json = "";

        //        var options = new JsonSerializerOptions();
        //        //options.Converters.Add(new NullableDateTimeConverter());

        //        if (data == null)
        //        {
        //            ErrorMsg = "Invalide Email";
        //            userViewModel.operationResult = OperationResult.FailureResult(ErrorMsg);

        //            json = JsonSerializer.Serialize(userViewModel, options);
        //        }
        //        else
        //        {
        //            userViewModel.operationResult = OperationResult.SuccessResult();
        //            userViewModel.applicationUser = data;


        //            json = JsonSerializer.Serialize(userViewModel, options);
        //        }

        //        return json;
        //        //var options = new JsonSerializerOptions();
        //        //options.Converters.Add(new NullableDateTimeConverter());

        //        //var json = JsonSerializer.Serialize(message, options);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}
    }
}
