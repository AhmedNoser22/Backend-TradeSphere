namespace TradeSphere.Application.Features.Customers.Deactivate;
public sealed class DeactivateCustomerCommandValidator : AbstractValidator<DeactivateCustomerCommand>
{
    public DeactivateCustomerCommandValidator() => RuleFor(x => x.CustomerId).NotEmpty();
}