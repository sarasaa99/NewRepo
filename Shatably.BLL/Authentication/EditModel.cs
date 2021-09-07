using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shatably.BLL.Authentication
{
    public class EditModel
    {
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
       
        [Required]
        public int MobileNumber { get; set; }
       
        [Required]
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public ContactRole? ContactRole { get; set; }

    }
}
