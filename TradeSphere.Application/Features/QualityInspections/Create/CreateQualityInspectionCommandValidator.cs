namespace TradeSphere.Application.Features.QualityInspections.Create;

public sealed class CreateQualityInspectionCommandValidator : AbstractValidator<CreateQualityInspectionCommand>
{
    public CreateQualityInspectionCommandValidator() => RuleFor(x => x.CustomsClearanceId).NotEmpty();
}