using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.PersistanceInterfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to,string subject,string body,string attachmentPath = null);
    }
}
