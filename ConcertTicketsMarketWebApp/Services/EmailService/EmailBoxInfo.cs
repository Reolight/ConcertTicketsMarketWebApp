using MimeKit;

namespace ConcertTicketsMarketWebApp.Services.EmailService
{
    public class EmailBoxInfo
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;


        public static explicit operator MailboxAddress(EmailBoxInfo receiver)
            => new MailboxAddress(receiver.Name, receiver.Email);
    }
}
