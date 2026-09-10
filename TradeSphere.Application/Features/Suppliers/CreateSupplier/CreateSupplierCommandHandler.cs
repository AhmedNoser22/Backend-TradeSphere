namespace TradeSphere.Application.Features.Suppliers.CreateSupplier;
public sealed class CreateSupplierCommandHandler(IRepository<Supplier> supplierRepository) : IRequestHandler<CreateSupplierCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = new Supplier(request.Name, request.Country, new ContactInfo(request.Email, request.Phone));
        await supplierRepository.AddAsync(supplier, cancellationToken);

        return Result<Guid>.Success(supplier.Id);
    }
}