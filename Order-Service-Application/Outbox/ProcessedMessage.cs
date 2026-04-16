namespace Order_Service_Application.Outbox
{
    public class ProcessedMessage
    {
        public Guid Id { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
