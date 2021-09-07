using Shatably.BLL.Model;
using Shatably.Data.Models;
using Shatably.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shatably.Service.ViewModel
{
    public class GetUserViewModel
    {
        public ApplicationUser applicationUser { get; set; }
        public OperationResult operationResult { get; set; }
    }
}
