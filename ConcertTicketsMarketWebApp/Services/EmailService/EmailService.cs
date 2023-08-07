using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Drawing;

namespace ConcertTicketsMarketWebApp.Services.EmailService
{
    public class EmailService : IEmailSender
    {
        private readonly EmailServiceConfigurations _configurations;

        public EmailService(IOptions<EmailServiceConfigurations> configurations)
            => _configurations = configurations.Value;

        //public async Task<bool> SendEmailAsync(EmailBoxInfo receiver, Email email)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add((MailboxAddress)_configurations.ServerMailBoxInfo);
        //    message.To.Add((MailboxAddress)receiver);
        //    (message.Subject, message.Body) = email;
        //    using var mailClient = new SmtpClient();
        //    try
        //    {
        //        mailClient.Connect(_configurations.MailServrerAddress, _configurations.MailServerPort, true);
        //        mailClient.Authenticate(_configurations.MailServerUsername, _configurations.MailServerPassword);
        //        await mailClient.SendAsync(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message, ex.StackTrace);
        //    }
        //    finally
        //    {
        //        mailClient.Disconnect(true);
        //    }

        //    return true;
        //}

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add((MailboxAddress)_configurations.ServerMailBoxInfo);
            message.To.Add(new MailboxAddress("", email));
            (message.Subject, message.Body) = 
                (subject, 
                new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage });

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
        }
    }
}
