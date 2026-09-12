namespace TradeSphere.Domain.Entities;
public sealed class QualityInspection : AuditableEntity
{
    public Guid CustomsClearanceId { get; private set; }
    public Guid PurchaseOrderId { get; private set; }
    public QualityInspectionStatus Status { get; private set; } = QualityInspectionStatus.Pending;
    public DateTimeOffset? CompletedAtUtc { get; private set; }

    private readonly List<QualityInspectionLine> _lines = [];
    public IReadOnlyCollection<QualityInspectionLine> Lines => _lines.AsReadOnly();

    private QualityInspection() { }

    public QualityInspection(Guid customsClearanceId, Guid purchaseOrderId)
    {
        CustomsClearanceId = customsClearanceId;
        PurchaseOrderId = purchaseOrderId;
    }

    public QualityInspectionLine RecordLine(Guid productId, int acceptedQuantity, int rejectedQuantity, int missingQuantity)
    {
        if (Status == QualityInspectionStatus.Completed)
            throw new BusinessRuleViolationException("Cannot add lines to a completed inspection.");

        Status = QualityInspectionStatus.InProgress;
        var line = new QualityInspectionLine(Id, productId, acceptedQuantity, rejectedQuantity, missingQuantity);
        _lines.Add(line);
        return line;
    }

    public void Complete()
    {
        if (_lines.Count == 0)
            throw new BusinessRuleViolationException("Cannot complete an inspection with no recorded lines.");

        Status = QualityInspectionStatus.Completed;
        CompletedAtUtc = DateTimeOffset.UtcNow;

        var acceptedLines = _lines
            .Where(l => l.AcceptedQuantity > 0)
            .Select(l => new QualityInspectionAcceptedLine(l.ProductId, l.AcceptedQuantity))
            .ToList();

        RaiseDomainEvent(new QualityInspectionCompletedEvent(Id, PurchaseOrderId, acceptedLines));
    }
}