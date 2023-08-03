using MailKit.Net.Smtp;
using MimeKit;

namespace ConcertTicketsMarketWebApp.Services.EmailService
{
    public class EmailService
    {
        private readonly EmailServiceConfigurations _configurations;

        public EmailService(EmailServiceConfigurations configurations)
            => _configurations = configurations;

        public async Task<bool> SendEmail(EmailBoxInfo receiver, Email email)
        {
            var message = new MimeMessage();
            message.From.Add((MailboxAddress)_configurations.ServerMailBoxInfo);
            message.To.Add((MailboxAddress)receiver);
            (message.Subject, message.Body) = email;
            using var mailClient = new SmtpClient();
            try
            {
                mailClient.Connect(_configurations.MailServrerAddress, _configurations.MailServerPort, true);
                mailClient.Authenticate(_configurations.MailServerUsername, _configurations.MailServerPassword);
                await mailClient.SendAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }
            finally
            {
                mailClient.Disconnect(true);
            }

            return true;
        }
    }
}
