using dMyBlogAPI.Models.EMail;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            throw new NotImplementedException();

            //using (var emailClient = new Pop3Client())
            //{
            //    emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);

            //    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

            //    emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);

            //    List<EmailMessage> emails = new List<EmailMessage>();
            //    for (int i = 0; i < emailClient.Count && i < maxCount; i++)
            //    {
            //        var message = emailClient.GetMessage(i);
            //        var emailMessage = new EmailMessage
            //        {
            //            Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
            //            Subject = message.Subject
            //        };
            //        emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
            //        emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
            //    }

            //    return emails;
            //}
        }

        public void Send(EmailMessage emailMessage)
        {




            var message = new MimeMessage();
            message.To.AddRange(emailMessage.To.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.Add(new MailboxAddress(_emailConfiguration.SmtAlias, _emailConfiguration.SmtpUsername));
            message.Subject = emailMessage.Subject;

            //string path = $"{Startup._env.WebRootPath}/templates/correo.html";
            //StreamReader sr = new StreamReader(path);
            //string cadenaHTML = sr.ReadToEnd();
            string cadenaHTML = "<div id='header' style='min-height:80px; background-color:black;color:white; text-align:center; vertical-align:middle; margin:auto'>" +
                    "<h1 style='margin:auto;line-height:80px;vertical-align:middle'>dMy Blog</h1>" +
                    "</div>" +
                    "<div id='body'style ='background-color:aliceblue; padding:50px'>" +
                    "{contenido}" +
                    "</div>" +
                    "<div id='footer' style='min-height:35px; background-color:black;color:white; text-align:center; vertical-align:middle'>" +
                    "<label style = 'margin:auto;line-height:30px;vertical-align:middle; font-size:xx-small'>&#9400;2019 dMy Blog</label>" +
                    "</div>";

            cadenaHTML = cadenaHTML.Replace("{contenido}", emailMessage.Content);
         //   sr.Close();


            string body = emailMessage.Content;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = cadenaHTML
            };


            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, _emailConfiguration.UseSSL);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                emailClient.Send(message);
                emailClient.Disconnect(true);
            }

        }
    }
}