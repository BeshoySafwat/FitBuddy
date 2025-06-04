using FitBuddy.Core.Services.Contract;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace FitBuddy.ApplicationServer.EmilService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendAsync(string From, string Recipients, string Subject, string Body)
        {
            var EmailSender = _configuration["EmailSetting:EmailSender"];
            var EmailPassword = _configuration["EmailSetting:EmailPassword"];

            var emailmessage = new MailMessage();
            emailmessage.From = new MailAddress(From);
            emailmessage.To.Add(Recipients);
            emailmessage.Subject = Subject;
            emailmessage.Body = $"<html><body>{Body}</body></html>";
            emailmessage.IsBodyHtml = true;
            using (var smtpClient = new SmtpClient(_configuration["EmailSetting:SMTPClientServer"], int.Parse(_configuration["EmailSetting:SMTPClientPort"]))) // Gmail SMTP server and port
            {
                smtpClient.Credentials = new NetworkCredential(EmailSender, EmailPassword);
                smtpClient.EnableSsl = true; // Enable SSL for secure connection
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                await smtpClient.SendMailAsync(emailmessage);
            }
        }
    }
}
