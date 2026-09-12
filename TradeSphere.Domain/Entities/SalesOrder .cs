namespace TradeSphere.Domain.Entities;
public sealed class SalesOrder : AuditableEntity
{
    public string OrderNumber { get; private set; } = default!;
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = default!;
    public SalesOrderStatus Status { get; private set; } = SalesOrderStatus.Draft;
    public DateTimeOffset OrderDate { get; private set; }

    private readonly List<SalesOrderLine> _lines = [];
    public IReadOnlyCollection<SalesOrderLine> Lines => _lines.AsReadOnly();

    public decimal TotalAmount => _lines.Sum(l => l.Quantity * l.UnitPrice);

    private SalesOrder() { }

    public SalesOrder(string orderNumber, Guid customerId, DateTimeOffset orderDate)
    {
        OrderNumber = orderNumber;
        CustomerId = customerId;
        OrderDate = orderDate;
    }

    public SalesOrderLine AddLine(Guid productId, int quantity, decimal unitPrice)
    {
        if (Status != SalesOrderStatus.Draft)
            throw new BusinessRuleViolationException("Lines can only be added while the order is still a Draft.");

        var line = new SalesOrderLine(Id, productId, quantity, unitPrice);
        _lines.Add(line);
        return line;
    }

    public void Confirm()
    {
        if (Status != SalesOrderStatus.Draft)
            throw new InvalidStateTransitionException(nameof(SalesOrder), Status.ToString(), SalesOrderStatus.Confirmed.ToString());
        if (_lines.Count == 0)
            throw new BusinessRuleViolationException("Cannot confirm a sales order with no lines.");

        Status = SalesOrderStatus.Confirmed;
    }
    public void MarkAsDelivered()
    {
        if (Status != SalesOrderStatus.Confirmed)
            throw new InvalidStateTransitionException(nameof(SalesOrder), Status.ToString(), SalesOrderStatus.Delivered.ToString());

        Status = SalesOrderStatus.Delivered;
        RaiseDomainEvent(new SalesOrderFulfilledEvent(Id, CustomerId, TotalAmount));
    }

    public void Cancel()
    {
        if (Status is SalesOrderStatus.Delivered or SalesOrderStatus.Cancelled)
            throw new InvalidStateTransitionException(nameof(SalesOrder), Status.ToString(), SalesOrderStatus.Cancelled.ToString());

        Status = SalesOrderStatus.Cancelled;
    }
}