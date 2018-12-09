using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LundSeating.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
      
        public Task SendEmailAsync(string email, string subject, string message)
        {
            string apikey = "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";

            return Execute(apikey, subject, message, email);

        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("LundSeating@dom.edu", "Lund Seating"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
    }
}