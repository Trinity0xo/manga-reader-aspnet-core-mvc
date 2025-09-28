namespace MangaReader.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string to, string subject, string content, bool isHtml = true);
        Task SendEmailFromTemplateAsync(string to, string subject, string templateName, string username, object value);
        Task SendResetPasswordLinkAsync(string username, string email, string resetPasswordToken);
    }
}
