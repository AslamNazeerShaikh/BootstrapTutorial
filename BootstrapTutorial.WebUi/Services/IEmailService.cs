using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BootstrapTutorial.WebUi.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string recipientEmail, string subject, string body, string cc, string bcc, string attachmentPath);
    }
}
