namespace Order_Service_Domain.Enums
{
    public enum OrderStatusEnum
    {
        Pending = 1,
        AwaitingStock = 2,
        StockUpdated = 3,
        AwaitingPayment = 4,
        Completed = 5,
        Cancelled = 6
    }
}
