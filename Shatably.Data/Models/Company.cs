using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shatably.Data.Models
{
    public class Company
    {
        public int CompanyId { get; set; }

        [StringLength(60)]
        public string CompanyName { get; set; }

        public string Photo { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(60)]
        public string City { get; set; }

        [StringLength(60)]
        public string Country { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }
        public string Mobile { get; set; }
        public string Hotline { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Gallery> Galleries { get; set; }
        public virtual ICollection<Reviews> Reviews { get; set; }
    }
}