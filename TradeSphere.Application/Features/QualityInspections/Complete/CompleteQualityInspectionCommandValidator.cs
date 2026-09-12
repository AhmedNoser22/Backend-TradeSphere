namespace TradeSphere.Application.Features.QualityInspections.Complete;

public sealed class CompleteQualityInspectionCommandValidator : AbstractValidator<CompleteQualityInspectionCommand>
{
    public CompleteQualityInspectionCommandValidator() => RuleFor(x => x.QualityInspectionId).NotEmpty();
}