using System;
using System.Collections.Generic;
using System.Text;

namespace Shatably.Data.Models
{
    public class Gallery
    {
        public int GalleryID { get; set; }
        public string Photo { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
