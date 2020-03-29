using HistorialClinico.Common.Configuration;
using HistorialClinico.Services.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace HistorialClinico.Services
{
    public class EmailSender: IEmailSender
    {
        private EmailSettings _emailSettings { get; }

        public EmailSender(IOptions<EmailSettings> configuration)
        {
            _emailSettings = configuration.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            Execute(email, subject, message).Wait();
            return Task.FromResult(0);
        }

        public async Task Execute(string email, string subject, string message)
        {
            var apiKey = _emailSettings.SendGridApiKey;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(email, _emailSettings.ApplicationName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email, _emailSettings.ApplicationName));
            await client.SendEmailAsync(msg);
        }
    }
}