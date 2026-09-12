namespace TradeSphere.Domain.Specifications;
public sealed class QualityInspectionByIdWithLinesSpecification : Specification<QualityInspection>
{
    public QualityInspectionByIdWithLinesSpecification(Guid id) : base(qi => qi.Id == id)
    {
        AddInclude(qi => qi.Lines);
    }
}