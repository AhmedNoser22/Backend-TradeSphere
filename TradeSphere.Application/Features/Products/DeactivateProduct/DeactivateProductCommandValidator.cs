namespace TradeSphere.Application.Features.Products.DeactivateProduct;

public sealed class DeactivateProductCommandValidator : AbstractValidator<DeactivateProductCommand>
{
    public DeactivateProductCommandValidator() => RuleFor(x => x.Id).NotEmpty();
}