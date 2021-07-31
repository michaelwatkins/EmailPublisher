using EmailPublisher.Logic;
using EmailPublisher.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace EmailPublisher
{
    public static class EmailService
    {
        private const string _requestCompleted = "Request to send email completed";
        private const string _requestTriggered = "Request to send email triggered";
        private static readonly EmailLogic _emailLogic;

        static EmailService()
        {
            // TODO:  convert from poor man's DI
            _emailLogic = new EmailLogic();
        }

        [FunctionName("SendEmail")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Log
            log.LogInformation(_requestTriggered);

            // Get our email request object from the body
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var emailRequest = JsonConvert.DeserializeObject<EmailRequest>(body);

            // Send the email
            var result = await _emailLogic.SendEmail(emailRequest);

            // Return an error if our operation failed
            if (!result.IsSuccessful) return new BadRequestResult();

            // Log
            log.LogInformation(_requestCompleted);

            // Return success! ...and the result
            return new OkObjectResult(result);
        }
    }
}
