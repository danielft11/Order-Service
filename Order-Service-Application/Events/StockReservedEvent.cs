namespace Order_Service_Application.Events
{
    public record StockReservedEvent(Guid MessageId, Guid OrderId);
}
