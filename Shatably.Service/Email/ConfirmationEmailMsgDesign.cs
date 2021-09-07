using System;
using System.Collections.Generic;
using System.Text;

namespace Shatably.Service.Email
{
    public static class ConfirmationEmailMsgDesign
    {
        public static string EmailMsgDesign(string EmailAddress, string confirmationLinkMsg)
        {
            string MailMsg = "Hello " + EmailAddress + ",<br />" +
                "<b>Welcome to Shatably,</b> <br />" +
                "Please Click the below link to activate your account <br />" +
                "<a href=" + confirmationLinkMsg + ">  Activate Your Email </a> <br /> Thanks :)";

            return MailMsg;
        }
    }
}
