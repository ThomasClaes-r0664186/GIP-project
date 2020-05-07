using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gip.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Gip.Controllers
{
    public class MailHandler
    {
        public void SendMail(ApplicationUser toEmailUser, string body, string subject) 
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Admin", "teamachtproject@gmail.com");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(toEmailUser.VoorNaam + " " + toEmailUser.Naam, toEmailUser.Email);
            message.To.Add(to);

            message.Subject = subject;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody =  body;

            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate("teamachtproject@gmail.com", "Xx*12345");

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }
    }
}
