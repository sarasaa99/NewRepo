using System;
using System.Collections.Generic;
using System.Text;

namespace Shatably.BLL.Authentication
{
    public static class UserRoles
    {
        public const string User = "User";
        public const string Admin = "Admin";
        public const string Company = "Company";
    }
    public enum ContactRole
    {
        Owner=1,
        CoOwner,
        Manager,
        Employee
    }
}
