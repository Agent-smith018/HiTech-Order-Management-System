using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace Project_module_4.Services
{
    public class EmailService
    {
        public void SendOrderConfirmation(string toEmail, string subject, string body)
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            var message = new MailMessage();
            message.From = new MailAddress(smtpSection.From);
            message.To.Add(toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            var client = new SmtpClient();
            client.Send(message);
        }
    }
}
