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

    public void Receive(int quantity, Guid referenceId, string note)
    {
        if (quantity <= 0) throw new BusinessRuleViolationException("Received quantity must be greater than zero.");

        QuantityOnHand += quantity;
        _movements.Add(new InventoryMovement(Id, InventoryMovementType.ReceiptFromQualityControl, quantity, referenceId, note));
    }

    public void IssueForSale(int quantity, Guid salesOrderId)
    {
        if (quantity <= 0) throw new BusinessRuleViolationException("Issued quantity must be greater than zero.");
        if (quantity > QuantityOnHand)
            throw new BusinessRuleViolationException($"Not enough stock: available {QuantityOnHand}, requested {quantity}.");

        QuantityOnHand -= quantity;
        _movements.Add(new InventoryMovement(Id, InventoryMovementType.SalesIssue, -quantity, salesOrderId, "Sales order fulfillment"));
    }
}