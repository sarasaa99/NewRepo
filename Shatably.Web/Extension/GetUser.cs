using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shatably.Web.Extension
{
    public static class GetUser
    {
        public static string GetUserByEmailClaim(this HttpContext httpContext)
        {
            string UserEmail = "";

            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                UserEmail = identity.FindFirst(ClaimTypes.Email).Value;

            }
            return UserEmail;

        }
    }
}
