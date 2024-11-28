using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BootstrapTutorial.WebUi.Models
{
    public class EmailSettings
    {
        public string? Email { get; set; }          // Sender's email address
        public string? Password { get; set; }       // Email account password or app password
        public string? Host { get; set; }           // SMTP server address
        public int Port { get; set; }               // SMTP server port
        public string? DisplayName { get; set; }    // Display name for the sender
    }
}
