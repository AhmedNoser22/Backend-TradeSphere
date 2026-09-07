namespace TradeSphere.Domain.Entities;

// One product + quantity + agreed price within a purchase order.
public sealed class PurchaseOrderLine : BaseEntity
{
    public Guid PurchaseOrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = default!;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string Currency { get; private set; } = default!;

    private PurchaseOrderLine() { }

    internal PurchaseOrderLine(Guid purchaseOrderId, Guid productId, int quantity, decimal unitPrice, string currency)
    {
        if (quantity <= 0) throw new BusinessRuleViolationException("Quantity must be greater than zero.");
        if (unitPrice < 0) throw new BusinessRuleViolationException("Unit price cannot be negative.");

        PurchaseOrderId = purchaseOrderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Currency = currency;
    }
}