namespace TradeSphere.Domain.Enums;

public enum SalesOrderStatus
{
    Draft = 1,
    Confirmed = 2,      // stock reserved/available, order accepted
    Delivered = 3,
    Cancelled = 4
}