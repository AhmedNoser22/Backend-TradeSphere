namespace TradeSphere.Application.Features.QualityInspections.RecordLine;
public sealed class RecordQualityInspectionLineCommandValidator : AbstractValidator<RecordQualityInspectionLineCommand>
{
    public RecordQualityInspectionLineCommandValidator()
    {
        RuleFor(x => x.QualityInspectionId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.AcceptedQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.RejectedQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MissingQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x)
            .Must(x => x.AcceptedQuantity + x.RejectedQuantity + x.MissingQuantity > 0)
            .WithMessage("At least one of Accepted/Rejected/Missing must be greater than zero.");
    }
}