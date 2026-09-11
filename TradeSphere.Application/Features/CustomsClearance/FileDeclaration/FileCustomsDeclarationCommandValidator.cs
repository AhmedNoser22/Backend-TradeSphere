namespace TradeSphere.Application.Features.CustomsClearance.FileDeclaration;
public sealed class FileCustomsDeclarationCommandValidator : AbstractValidator<FileCustomsDeclarationCommand>
{
    public FileCustomsDeclarationCommandValidator()
    {
        RuleFor(x => x.CustomsClearanceId).NotEmpty();
        RuleFor(x => x.DeclarationNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Port).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DeclaredGoodsValue).GreaterThanOrEqualTo(0);
    }
}