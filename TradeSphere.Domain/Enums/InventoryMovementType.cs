namespace TradeSphere.Domain.Enums;

// Every change to stock quantity is recorded as a movement, never a direct
// edit to a "quantity" field, so we always have a full audit trail.
public enum InventoryMovementType
{
    ReceiptFromQualityControl = 1, // goods accepted by QC, entering the warehouse
    SalesIssue = 2,                // goods leaving because of a sale
    AdjustmentIn = 3,              // manual correction, e.g. stock count found extra
    AdjustmentOut = 4
}