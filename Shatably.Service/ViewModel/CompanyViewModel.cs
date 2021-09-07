using Shatably.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shatably.Service.Model
{
    public class CompanyViewModel
    {
        public Company company { get; set; }
        public ICollection<Gallery> galleries { get; set; }
        public ICollection<Reviews> reviews { get; set; }
    }
}