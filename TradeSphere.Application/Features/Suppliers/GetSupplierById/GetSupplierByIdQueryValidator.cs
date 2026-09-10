namespace TradeSphere.Application.Features.Suppliers.GetSupplierById;

public sealed class GetSupplierByIdQueryValidator : AbstractValidator<GetSupplierByIdQuery>
{
    public GetSupplierByIdQueryValidator() => RuleFor(x => x.Id).NotEmpty();
}