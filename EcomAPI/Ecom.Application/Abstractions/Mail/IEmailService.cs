namespace Ecom.Application.Abstractions.Mail
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
