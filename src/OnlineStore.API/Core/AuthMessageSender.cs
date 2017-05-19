using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.API.Core
{
    public class AuthMessageSender : IEmailSender
    {
        public Task SendEmail(string Email, string Subject, string Message)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Administrator", "areef.hide@gmail.com"));
            message.To.Add(new MailboxAddress("user", Email));
            message.Subject = Subject;
            message.Body = new TextPart("Html") { Text = Message };
            using(var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("areef.hide@gmail.com", "fir3horse");
                client.Send(message);
                client.Disconnect(true);
            }

            return Task.FromResult(0);
        }
    }
}
