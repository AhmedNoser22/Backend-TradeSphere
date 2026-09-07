namespace TradeSphere.Domain.Entities;

public sealed class Payment : AuditableEntity
{
    public PaymentDirection Direction { get; private set; }
    public Guid ReferenceOrderId { get; private set; } // SalesOrderId or PurchaseOrderId depending on Direction
    public decimal TotalDue { get; private set; }
    public decimal AmountPaid { get; private set; }
    public string Currency { get; private set; } = default!;
    public PaymentStatus Status { get; private set; } = PaymentStatus.Unpaid;

    public decimal RemainingBalance => TotalDue - AmountPaid;

    private Payment() { }

    public Payment(PaymentDirection direction, Guid referenceOrderId, decimal totalDue, string currency)
    {
        if (totalDue < 0) throw new BusinessRuleViolationException("Total due cannot be negative.");

        Direction = direction;
        ReferenceOrderId = referenceOrderId;
        TotalDue = totalDue;
        Currency = currency;
    }

    public void RecordInstallment(decimal amount)
    {
        if (amount <= 0) throw new BusinessRuleViolationException("Payment amount must be greater than zero.");
        if (Status == PaymentStatus.FullyPaid)
            throw new BusinessRuleViolationException("This payment is already fully settled.");
        if (AmountPaid + amount > TotalDue)
            throw new BusinessRuleViolationException("Payment amount exceeds the remaining balance.");

        AmountPaid += amount;
        Status = AmountPaid == TotalDue ? PaymentStatus.FullyPaid : PaymentStatus.PartiallyPaid;
    }
}