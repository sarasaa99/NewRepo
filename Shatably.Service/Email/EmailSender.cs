using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shatably.Service.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration emailConfiguration;

        public EmailSender(EmailConfiguration emailConfiguration)
        {
            this.emailConfiguration = emailConfiguration;
        }
        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.Port, SecureSocketOptions.Auto);
                    //client.AuthenticationMechanisms.Remove("XOAUTH2");
                    
                    await client.AuthenticateAsync(emailConfiguration.UserName, emailConfiguration.Password);


                    
                    await client.SendAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("{0}", message.Content) };

            return emailMessage;
        }
    }
}
