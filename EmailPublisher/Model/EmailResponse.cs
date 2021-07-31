namespace EmailPublisher.Model
{
    public class EmailResponse
    {
        /// <summary>
        /// Convenience constructor
        /// </summary>
        public EmailResponse(string recipient, string messageSent)
        {
            Recipient   = recipient;
            MessageSent = messageSent;
        }

        public string Recipient { get; set; }
        public string MessageSent { get; set; }
    }
}
