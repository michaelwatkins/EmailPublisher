namespace EmailPublisher.Model
{
    public class Status<T>
    {
        /// <summary>
        /// Convenience constructor
        /// </summary>
        public Status(bool isSuccessful, string message, T payLoad)
        {
            IsSuccessful = isSuccessful;
            Message      = message;
            PayLoad      = payLoad;
        }

        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public T PayLoad { get; set; }
    }
}
