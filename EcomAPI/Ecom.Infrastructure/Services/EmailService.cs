using Ecom.Application.Abstractions.Mail;
using Ecom.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Ecom.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var fromAddress= new MailAddress(_settings.FromEmail, _settings.FromName);
            var toAddress = new MailAddress(toEmail);

            var smtp = new SmtpClient(_settings.SmtpServer, _settings.Port)
            {
                EnableSsl = _settings.EnableSsl,
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            if (!string.IsNullOrWhiteSpace(_settings.ReplyToEmail))
            {
                message.ReplyToList.Add(new MailAddress(_settings.ReplyToEmail));
            }

            await smtp.SendMailAsync(message);
        }
    }
}
