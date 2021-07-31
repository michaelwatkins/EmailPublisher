using EmailPublisher.Model;
using System.Collections.Generic;
using System.Linq;

namespace EmailPublisher.Logic
{
    public class EmailLogic
    {
        private const string _requestNotSupplied = "Email request is not supplied";
        private const string _notAuthorized      = "Not authorized to send emails to this email address";
        private const string _errorOccured       = "Error occured";
        private const string _success            = "Successfully sent the email";

        public Status<EmailResponse> SendEmail(EmailRequest emailRequest)
        {
            // Validate
            var validationResult = ValidateEmailRequest(emailRequest);

            // Failed validation
            if (!validationResult.IsValid)
            {
                return new Status<EmailResponse>(false, validationResult.Message, null);
            }

            // Send Email
            try
            {
                // TODO:  get an smtp server - could not create SendGrid account in Azure - service problems?
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
                return new List<string> { "mikemcsecertified@hotmail.com" };
            }
        }
    }

    //Create a Azure Function
    //Create a POST endpoint that takes an email address and a name
    //Send that person an email with a body that contains their name and whatever else
    //Probably limit it to just @pfl and your own email address and return a 400-level status otherwise
    //Send us a link to the live service and a sample payload and we’ll test it out.
    //You should be able to sign up for a trial period of azure that provides a fair amount of resources for free, but I can also set up an acct for you if that’d be preferable
    //Send us a link to a bitbucket/github/dropbox/etc repository where we can look at your code
}
