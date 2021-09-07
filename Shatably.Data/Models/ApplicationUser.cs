using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shatably.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int MobileNumber { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactRole { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<Appartment> Appartments { get; set; }
        public virtual ICollection<Reviews> CompanyReview { get; set; }
        public virtual ICollection<Reviews> UserReview { get; set; }
    }
}
