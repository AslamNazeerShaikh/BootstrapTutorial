using System.Diagnostics;
using BootstrapTutorial.WebUi.Models;
using BootstrapTutorial.WebUi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BootstrapTutorial.WebUi.Controllers
{
    [Route("[controller]")]
    public class EmailController : Controller
    {
        private readonly ILogger<EmailController> _logger;

        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmailController(ILogger<EmailController> logger, IEmailService emailService, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string recipientEmail, string cc, string bcc, string body)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "files", "dummy.txt");
            await _emailService.SendEmailAsync(
                recipientEmail,
                "Test Email from ASP.NET Core",
                body,
                cc,
                bcc,
                filePath
            );

            ViewBag.Message = "Email Sent Successfully!";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
