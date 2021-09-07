using System;
using System.Collections.Generic;
using System.Text;

namespace Shatably.Data.Models
{
    public class Reviews
    {
        public int ReviewsId { get; set; }
        public string Massege { get; set; }
        public double Rate { get; set; }
        public virtual ApplicationUser CompanyIDUser { get; set; }
        public virtual ApplicationUser UserID { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
