using EmailPublisher.Model;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace EmailPublisher.Service
{
    public class EmailClient
    {
        // TODO:  store in Azure app settings
        private const string _apiKey         = "nope";
        private const string _from           = "mikemcsecertified@hotmail.com";
        private const string _fromName       = "Support";
        private const string _defaultSubject = "Message from Michael's Email Service";


        public async Task SendEmail(EmailRequest emailRequest)
        {
            // Email client
            var client = new SendGridClient(_apiKey);

            // Constructing the email message
            var msg = new SendGridMessage()
            {
                From             = new EmailAddress(_from, _fromName),
                Subject          = _defaultSubject,
                PlainTextContent = emailRequest.Message,
                HtmlContent      = emailRequest.Message
            };
            msg.AddTo(new EmailAddress(emailRequest.EmailAddress, emailRequest.Name));

            // Send the email
            var response = await client.SendEmailAsync(msg);
        }
    }
}
