namespace TradeSphere.Application.Features.Suppliers.DeactivateSupplier;
public sealed class DeactivateSupplierCommandValidator : AbstractValidator<DeactivateSupplierCommand>
{
    public DeactivateSupplierCommandValidator() => RuleFor(x => x.Id).NotEmpty();
}