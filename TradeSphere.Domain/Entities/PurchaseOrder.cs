namespace TradeSphere.Domain.Entities;
public sealed class PurchaseOrder : AuditableEntity
{
    public string OrderNumber { get; private set; } = default!;
    public Guid SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = default!;
    public PurchaseOrderStatus Status { get; private set; } = PurchaseOrderStatus.Draft;
    public DateTimeOffset OrderDate { get; private set; }

    private readonly List<PurchaseOrderLine> _lines = [];
    public IReadOnlyCollection<PurchaseOrderLine> Lines => _lines.AsReadOnly();

    private PurchaseOrder() { }

    public PurchaseOrder(string orderNumber, Guid supplierId, DateTimeOffset orderDate)
    {
        OrderNumber = orderNumber;
        SupplierId = supplierId;
        OrderDate = orderDate;
    }

    public PurchaseOrderLine AddLine(Guid productId, int quantity, decimal unitPrice, string currency)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new BusinessRuleViolationException("Lines can only be added while the order is still a Draft.");

        if (_lines.Count != 0 && _lines[0].Currency != currency)
            throw new BusinessRuleViolationException($"All lines on a purchase order must use the same currency ({_lines[0].Currency}).");

        var line = new PurchaseOrderLine(Id, productId, quantity, unitPrice, currency);
        _lines.Add(line);
        return line;
    }

    public void Confirm()
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidStateTransitionException(nameof(PurchaseOrder), Status.ToString(), PurchaseOrderStatus.Confirmed.ToString());
        if (_lines.Count == 0)
            throw new BusinessRuleViolationException("Cannot confirm a purchase order with no lines.");

        Status = PurchaseOrderStatus.Confirmed;
    }

    public void MarkPartiallyShipped()
    {
        if (Status is not (PurchaseOrderStatus.Confirmed or PurchaseOrderStatus.PartiallyShipped))
            throw new InvalidStateTransitionException(nameof(PurchaseOrder), Status.ToString(), PurchaseOrderStatus.PartiallyShipped.ToString());

        Status = PurchaseOrderStatus.PartiallyShipped;
    }

    public void MarkFullyShipped()
    {
        if (Status is not (PurchaseOrderStatus.Confirmed or PurchaseOrderStatus.PartiallyShipped))
            throw new InvalidStateTransitionException(nameof(PurchaseOrder), Status.ToString(), PurchaseOrderStatus.FullyShipped.ToString());

        Status = PurchaseOrderStatus.FullyShipped;
    }

    public void Close()
    {
        if (Status != PurchaseOrderStatus.FullyShipped)
            throw new InvalidStateTransitionException(nameof(PurchaseOrder), Status.ToString(), PurchaseOrderStatus.Closed.ToString());

        Status = PurchaseOrderStatus.Closed;
    }

    public void Cancel()
    {
        if (Status is PurchaseOrderStatus.Closed or PurchaseOrderStatus.Cancelled)
            throw new InvalidStateTransitionException(nameof(PurchaseOrder), Status.ToString(), PurchaseOrderStatus.Cancelled.ToString());

        Status = PurchaseOrderStatus.Cancelled;
    }
}