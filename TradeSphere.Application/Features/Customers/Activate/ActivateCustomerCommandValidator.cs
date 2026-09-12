namespace TradeSphere.Application.Features.Customers.Activate;

public sealed class ActivateCustomerCommandValidator : AbstractValidator<ActivateCustomerCommand>
{
    public ActivateCustomerCommandValidator() => RuleFor(x => x.CustomerId).NotEmpty();
}