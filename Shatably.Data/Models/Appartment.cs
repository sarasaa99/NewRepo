using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shatably.Data.Models
{
    public class Appartment
    {
        public int AppartmentID { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int Floor { get; set; }
        [Required]
        public int AppartmentNumber { get; set; }
        public int Rooms { get; set; }
        public int Bathrooms { get; set; }
        [Required]
        public double Totalsize { get; set; }
        [Required]
        public string Dimensions { get; set; }
        public string Photo1 { get; set; }
        public string Photo2 { get; set; }

        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
