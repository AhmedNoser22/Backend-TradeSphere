namespace TradeSphere.Domain.Entities;
public sealed class InventoryMovement : BaseEntity
{
    public Guid InventoryStockId { get; private set; }
    public InventoryMovementType Type { get; private set; }
    public int Quantity { get; private set; }
    public Guid ReferenceId { get; private set; } // e.g. QualityInspectionId or SalesOrderId
    public string Note { get; private set; } = default!;
    public DateTimeOffset OccurredAtUtc { get; private set; } = DateTimeOffset.UtcNow;

    private InventoryMovement() { }

    internal InventoryMovement(Guid inventoryStockId, InventoryMovementType type, int quantity, Guid referenceId, string note)
    {
        InventoryStockId = inventoryStockId;
        Type = type;
        Quantity = quantity;
        ReferenceId = referenceId;
        Note = note;
    }
}