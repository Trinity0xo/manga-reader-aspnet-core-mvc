using System.Net;
using System.Net.Mail;

namespace MangaReader.Services.Implementations
{
    public class MailService : IMailService
    {
        private readonly IAppConfigService _appConfig;
        private readonly ILogger<MailService> _logger;

        public MailService(IAppConfigService appConfig, ILogger<MailService> logger)
        {
            _appConfig = appConfig;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string content, bool isHtml)
        {
            try
            {
                string fromAddress = _appConfig.GetMailUsername()!;
                string username = _appConfig.GetMailUsername()!;
                string password = _appConfig.GetMailPassword()!;
                string host = _appConfig.GetMailHost()!;
                int port = int.Parse(_appConfig.GetMailPort()!);

                var message = new MailMessage(fromAddress, to)
                {
                    Subject = subject,
                    Body = content,
                    IsBodyHtml = isHtml
                };

                var smtp = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };

                await smtp.SendMailAsync(message);
                _logger.LogInformation("Email send to '{To}' with subject '{Subject}' ", to, subject);
            }
            catch (Exception e) {
                _logger.LogError(e, "Failed to send email to {To} with subject '{Subject}'", to, subject);
            }
        }

        public async Task SendEmailFromTemplateAsync(string to, string subject, string templateName, string username, object value)
        {
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", templateName + ".html");
            if (File.Exists(templatePath))
            {
                string content = await File.ReadAllTextAsync(templatePath);

                content = content.Replace("{{name}}", username);
                content = content.Replace("{{value}}", value?.ToString());

                await SendEmailAsync(to, subject, content, true);
            }
            else
            {
                _logger.LogError("Failed to send mail. Template '{Template}' not found at path: {Path}", templateName, templatePath);
            }
        }

        public async Task SendResetPasswordLinkAsync(string username, string email, string callbackUrl)
        {
            await SendEmailFromTemplateAsync(
                to: email,
                subject: "Reset Password",
                templateName: "reset_password_email",
                username: username,
                value: callbackUrl
            );
        }
    }
}
