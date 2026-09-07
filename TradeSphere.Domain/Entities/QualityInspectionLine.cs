namespace TradeSphere.Domain.Entities;
public sealed class QualityInspectionLine : BaseEntity
{
    public Guid QualityInspectionId { get; private set; }
    public Guid ProductId { get; private set; }
    public int AcceptedQuantity { get; private set; }
    public int RejectedQuantity { get; private set; }
    public int MissingQuantity { get; private set; }

    private QualityInspectionLine() { }

    internal QualityInspectionLine(Guid qualityInspectionId, Guid productId, int acceptedQuantity, int rejectedQuantity, int missingQuantity)
    {
        if (acceptedQuantity < 0 || rejectedQuantity < 0 || missingQuantity < 0)
            throw new BusinessRuleViolationException("Quantities cannot be negative.");

        QualityInspectionId = qualityInspectionId;
        ProductId = productId;
        AcceptedQuantity = acceptedQuantity;
        RejectedQuantity = rejectedQuantity;
        MissingQuantity = missingQuantity;
    }
}