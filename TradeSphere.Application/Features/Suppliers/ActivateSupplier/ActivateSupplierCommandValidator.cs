namespace TradeSphere.Application.Features.Suppliers.ActivateSupplier;

public sealed class ActivateSupplierCommandValidator : AbstractValidator<ActivateSupplierCommand>
{
    public ActivateSupplierCommandValidator() => RuleFor(x => x.Id).NotEmpty();
}