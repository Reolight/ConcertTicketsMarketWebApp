using MimeKit;
using MimeKit.Text;

namespace EmailService
{
    public class Email
    {
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public TextFormat TextFormat { get; set; } = TextFormat.Plain;

        public void Deconstruct(out string subject, out TextPart body)
        {
            subject = Subject;
            body = new TextPart(TextFormat)
            {
                Text = Body
            };
        }
    }
}
