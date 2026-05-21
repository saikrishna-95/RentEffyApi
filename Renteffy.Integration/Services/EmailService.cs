using Microsoft.Extensions.Configuration;
using MimeKit;
using Renteffy.Domain.Services.PersistanceInterfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Renteffy.Domain.Services.PersistanceInterfaces.Services;

namespace Renteffy.Integration.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string to,string subject,string body,string attachmentPath = null)
        {
            var email = new MimeMessage();

            email.From.Add(
                MailboxAddress.Parse(_config["EmailSettings:FromEmail"]));

            email.To.Add(MailboxAddress.Parse(to));

            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };

            if (!string.IsNullOrEmpty(attachmentPath))
            {
                builder.Attachments.Add(attachmentPath);
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(_config["EmailSettings:Host"],Convert.ToInt32(_config["EmailSettings:Port"]),false);

            await smtp.AuthenticateAsync(_config["EmailSettings:Username"],_config["EmailSettings:Password"]);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}
