namespace TradeSphere.Domain.Entities;

public sealed class SalesOrderLine : BaseEntity
{
    public Guid SalesOrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = default!;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    private SalesOrderLine() { }

    internal SalesOrderLine(Guid salesOrderId, Guid productId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0) throw new BusinessRuleViolationException("Quantity must be greater than zero.");
        if (unitPrice < 0) throw new BusinessRuleViolationException("Unit price cannot be negative.");

        SalesOrderId = salesOrderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}