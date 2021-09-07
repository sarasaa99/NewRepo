using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Shatably.BLL;
using Shatably.BLL.Authentication;
using Shatably.Data.Models;
using Shatably.Service;
using Shatably.Service.Model;
using Shatably.Web.Extension;

namespace Shatably.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IConfiguration config;

        public AccountController(IAccountService accountService, IConfiguration config)
        {
            this.accountService = accountService;
            this.config = config;
        }

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterUser(RegisterModel User)
        {
            try
            {
                var result = await accountService.RegisterUser(User);
               
                if (!result.Succeeded)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError,
                       result.Message);
                }
                return StatusCode(StatusCodes.Status200OK,
                    result.Message);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [AllowAnonymous]
        [HttpPost("RegisterCompany")]
        public async Task<ActionResult> RegisterCompany(RegisterModelCompany User)
        {
            try
            {
                var result = await accountService.RegisterCompany(User);

                if (!result.Succeeded)
                {
                    
                    return StatusCode(StatusCodes.Status500InternalServerError,
                       result.Message);
                }
                return StatusCode(StatusCodes.Status200OK,
                    result.Message);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [AllowAnonymous]
        [HttpPost("RegisterAdmin")]
        public async Task<ActionResult> RegisterAdmin(RegisterModel User)
        {
            try
            {
                var result = await accountService.RegisterAdmin(User);

                if (!result.Succeeded)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError,
                       result.Message);
                }
                return StatusCode(StatusCodes.Status200OK,
                    result.Message);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<ApplicationUser>> Login(LoginModel User)
        {
            try
            {
                var result = await accountService.Login(User);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        result.Message);
                }

                var tokenstring = result.Data;

               return Ok(new { token = tokenstring });
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(string UserId, string token)
        {
            try
            {
                var result = await accountService.ConfimEmail(UserId, token);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        result.Message);
                }
                

                return StatusCode(StatusCodes.Status200OK,
                   result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword(string Email)
        {
            try
            {
                var result = await accountService.ForgotPassword(Email);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        result.Message);
                }


                return StatusCode(StatusCodes.Status200OK,
                   result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel resetPasswordModel, string token)
        {
            try
            {
                var result = await accountService.ResetPassword(resetPasswordModel, token);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        result.Message);
                }


                return StatusCode(StatusCodes.Status200OK,
                   result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [Authorize(Roles = "User, Admin, Company")]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(string Userid, ChangePasswordModel changePasswordModel)
        {
            try
            {
                var result = await accountService.ChnagePassword(changePasswordModel, Userid);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        result.Message);
                }


                return StatusCode(StatusCodes.Status200OK,
                   result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        //[Authorize(Roles = "User, Admin, Company")]
        //[HttpGet]
        //public async Task<ActionResult<string>> GetUser()
        //{
        //    string UserEmail = "";

        //    UserEmail = HttpContext.GetUserByEmailClaim();

        //    var data = await accountService.GetUserByEmail(UserEmail);

        //    return data;

        //}
        [Authorize(Roles = "User, Admin, Company")]
        [HttpGet("GetUser")]
        public async Task<ActionResult<ApplicationUser>> GetUser()
        {
            string UserEmail = "";

            UserEmail = HttpContext.GetUserByEmailClaim();

            var result = await accountService.GetUserByEmail(UserEmail);

            if(!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       result.Message);
            }

            return result.Data;

        }
        [HttpPut]
        [Authorize(Roles = "User, Admin, Company")]
        [Route("UpdateUser")]
        public async Task<ActionResult> UpdateUser(EditModel UpdatedUser)
        {
            try
            {
                var result = await accountService.UpdateUser(UpdatedUser);

                if (!result.Succeeded)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError,
                       result.Message);
                }
                return StatusCode(StatusCodes.Status200OK,
                    result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("DeleteUser")]
        public async Task<ActionResult> DeleteUser(string Id)
        {
            try
            {
                var result = await accountService.DeleteUser(Id);
                if (!result.Succeeded)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError,
                       result.Message);
                }
                return StatusCode(StatusCodes.Status200OK,
                    result.Message);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

    }
}