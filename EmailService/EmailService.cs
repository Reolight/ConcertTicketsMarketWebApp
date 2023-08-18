using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceConfigurations _configurations;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailServiceConfigurations> configurations, ILogger<EmailService> logger)
            => (_configurations, _logger) = (configurations.Value, logger);

        public async Task<bool> SendEmailAsync(EmailBoxInfo receiver, Email email)
        {
            var message = new MimeMessage();
            message.From.Add((MailboxAddress)_configurations.ServerMailBoxInfo);
            message.To.Add((MailboxAddress)receiver);
            (message.Subject, message.Body) = email;
            using var mailClient = new SmtpClient();
            try
            {
                await mailClient.ConnectAsync(_configurations.MailServerAddress, _configurations.MailServerPort, SecureSocketOptions.StartTls);
                await mailClient.AuthenticateAsync(_configurations.MailServerUsername, _configurations.MailServerPassword);
                await mailClient.SendAsync(message);
                _logger.LogInformation("Email is sent to {ReceiverEmail}", receiver.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured: {ExMessage}", ex.Message);
            }
            finally
            {
                await mailClient.DisconnectAsync(true);
            }

            return true;
        }
    }
}
