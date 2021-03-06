using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shatably.BLL.Authentication
{
    public class RegisterModel
    {
       // [Required]
        public string FirstName { get; set; }
       // [Required]
        public string LastName { get; set; }
       // [Required]
        public string UserName { get; set; }
       // [Required]
   //     [DataType(DataType.Password)]
        public string Password { get; set; }
        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password",
        //    ErrorMessage = "Password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
       // [Required]
        public int MobileNumber { get; set; }
       // [Required]
     //   [EmailAddress]
        public string EmailAddress { get; set; }
       // [Required]
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public ContactRole? ContactRole { get; set; }

    }
}
