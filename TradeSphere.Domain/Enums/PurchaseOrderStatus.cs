namespace TradeSphere.Domain.Enums;
public enum PurchaseOrderStatus
{
    Draft = 1,      // just created, not sent/confirmed with the supplier yet
    Confirmed = 2,  // agreed with supplier, waiting to be shipped
    PartiallyShipped = 3,
    FullyShipped = 4,
    Closed = 5,     // fully received into warehouse (after QC) and reconciled
    Cancelled = 6
}   