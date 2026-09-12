namespace TradeSphere.Domain.Specifications;
public sealed class QualityInspectionByCustomsClearanceIdSpecification : Specification<QualityInspection>
{
    public QualityInspectionByCustomsClearanceIdSpecification(Guid customsClearanceId)
        : base(qi => qi.CustomsClearanceId == customsClearanceId) { }
}