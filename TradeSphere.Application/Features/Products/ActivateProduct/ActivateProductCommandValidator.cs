namespace TradeSphere.Application.Features.Products.ActivateProduct;

public sealed class ActivateProductCommandValidator : AbstractValidator<ActivateProductCommand>
{
    public ActivateProductCommandValidator() => RuleFor(x => x.Id).NotEmpty();
}