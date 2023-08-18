using MimeKit;

namespace EmailService
{
    public class EmailServiceConfigurations
    {
        public EmailBoxInfo ServerMailBoxInfo { get; set; } = null!;
        public string MailServerAddress { get; set; } = null!;
        public int MailServerPort { get; set; } = 0;
        public string MailServerUsername { get; set;} = null!;
        // I thought a litte about SecureString and not sure if it'll fit here...
        public string MailServerPassword { get; set;} = null!;

    }
}
