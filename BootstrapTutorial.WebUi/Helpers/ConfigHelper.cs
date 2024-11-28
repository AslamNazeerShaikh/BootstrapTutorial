using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootstrapTutorial.WebUi.Models;

namespace BootstrapTutorial.WebUi.Helpers
{
    public class ConfigHelper
    {
        private readonly IConfiguration _configuration;

        public ConfigHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EmailSettings GetEmailSettings()
        {
            var settings = new EmailSettings();
            _configuration.GetSection("EmailSettings").Bind(settings);
            return settings;
        }
    }
}
