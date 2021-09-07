using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shatably.Service.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}
