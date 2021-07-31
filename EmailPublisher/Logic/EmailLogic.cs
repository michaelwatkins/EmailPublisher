using EmailPublisher.Model;
using EmailPublisher.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailPublisher.Logic
{
    public class EmailLogic
    {
        private readonly EmailClient _emailClient;
        private const string _requestNotSupplied = "Email request is not supplied";
        private const string _notAuthorized      = "Not authorized to send emails to this email address";
        private const string _errorOccured       = "Error occured";
        private const string _success            = "Successfully sent the email";

        public EmailLogic()
        {
            // TODO:  remove poor man's DI
            _emailClient = new EmailClient();
        }

        public async Task<Status<EmailResponse>> SendEmail(EmailRequest emailRequest)
        {
            // Validate
            var validationResult = ValidateEmailRequest(emailRequest);

            // Failed validation
            if (!validationResult.IsValid)
            {
                return new Status<EmailResponse>(false, validationResult.Message, null);
            }

            // Send the email
            try
            {
                await _emailClient.SendEmail(emailRequest);
            }
            catch
            {
                return new Status<EmailResponse>(false, _errorOccured, null);
            }

            // Return success!
            return new Status<EmailResponse>(true, _success, new EmailResponse(emailRequest.EmailAddress, emailRequest.Message));
        }

        public ValidationSummary ValidateEmailRequest(EmailRequest emailRequest)
        {
            // Rule 1:  required
            if (emailRequest == null)
            {
                return new ValidationSummary() { IsValid = false, Message = _requestNotSupplied };
            }

            // Normalize the data
            var emailAddress = emailRequest.EmailAddress.ToLower();

            // Rule 2: Must be PFL domain or Michael's email
            if (!ValidDomains.Any(x => emailAddress.Contains(x)) && !FullAccessEmails.Contains(emailAddress))
            {
                return new ValidationSummary() { IsValid = false, Message = _notAuthorized };
            }

            // Rule 3: TODO - Validate this is a valid email address

            // Return success
            return new ValidationSummary { IsValid = true, Message = string.Empty };
        }

        private List<string> ValidDomains
        {
            get
            {
                return new List<string> { "@pfl.com" };
            }
        }

        private List<string> FullAccessEmails
        {
            get
            {
                return new List<string> { "mikemcsecertified@hotmail.com", "mikemcsecertified@gmail.com" };
            }
        }
    }
}
