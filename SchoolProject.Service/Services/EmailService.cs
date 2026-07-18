using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;



        public EmailService(IOptions<MailSettings> options)
        {
            _mailSettings= options.Value;
        }
        public async Task<string> SendEmailAsync(string email, string message, string? subject)
        {
            try
            {
                var mail = new MimeMessage() ;

                mail.From.Add(new MailboxAddress(
                    _mailSettings.DisplayName,
                    _mailSettings.Email));

                mail.To.Add(MailboxAddress.Parse(email));

                mail.Subject = subject==null? "School Project":subject;

                mail.Body = new TextPart("html")
                {
                    Text = message
                };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(
                    _mailSettings.Host,
                    _mailSettings.Port,
                    _mailSettings.EnableSSL
                        ? SecureSocketOptions.StartTls
                        : SecureSocketOptions.None);

                await smtp.AuthenticateAsync(
                    _mailSettings.Email,
                    _mailSettings.Password);

                await smtp.SendAsync(mail);

                await smtp.DisconnectAsync(true);

                return "EmailSentSuccessfully";
            
            }
            catch(Exception ex) 
            {
                return "FailedToEmailSent";
            }
        }
    }
}
