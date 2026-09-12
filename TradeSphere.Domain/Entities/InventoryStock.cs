namespace TradeSphere.Domain.Entities;
public sealed class InventoryStock : AuditableEntity
{
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = default!;
    public int QuantityOnHand { get; private set; }

    private readonly List<InventoryMovement> _movements = [];
    public IReadOnlyCollection<InventoryMovement> Movements => _movements.AsReadOnly();

    private InventoryStock() { }

    public InventoryStock(Guid productId) => ProductId = productId;

    public InventoryMovement Receive(int quantity, Guid referenceId, string note)
    {
        if (quantity <= 0) throw new BusinessRuleViolationException("Received quantity must be greater than zero.");

        QuantityOnHand += quantity;
        var movement = new InventoryMovement(Id, InventoryMovementType.ReceiptFromQualityControl, quantity, referenceId, note);
        _movements.Add(movement);
        return movement;
    }

    public InventoryMovement IssueForSale(int quantity, Guid salesOrderId)
    {
        if (quantity <= 0) throw new BusinessRuleViolationException("Issued quantity must be greater than zero.");
        if (quantity > QuantityOnHand)
            throw new BusinessRuleViolationException($"Not enough stock: available {QuantityOnHand}, requested {quantity}.");

        QuantityOnHand -= quantity;
        var movement = new InventoryMovement(Id, InventoryMovementType.SalesIssue, -quantity, salesOrderId, "Sales order fulfillment");
        _movements.Add(movement);
        return movement;
    }

    public InventoryMovement AdjustQuantity(int quantityChange, string reason)
    {
        if (quantityChange == 0)
            throw new BusinessRuleViolationException("Adjustment quantity cannot be zero.");
        if (QuantityOnHand + quantityChange < 0)
            throw new BusinessRuleViolationException($"Adjustment would result in negative stock (current: {QuantityOnHand}, change: {quantityChange}).");

        QuantityOnHand += quantityChange;
        var type = quantityChange > 0 ? InventoryMovementType.AdjustmentIn : InventoryMovementType.AdjustmentOut;
        var movement = new InventoryMovement(Id, type, quantityChange, Id, reason);
        _movements.Add(movement);
        return movement;
    }
}