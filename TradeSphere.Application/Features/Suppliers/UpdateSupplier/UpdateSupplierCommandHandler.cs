namespace TradeSphere.Application.Features.Suppliers.UpdateSupplier;

public sealed class UpdateSupplierCommandHandler(IRepository<Supplier> supplierRepository) : IRequestHandler<UpdateSupplierCommand, Result>
{
    public async Task<Result> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Supplier), request.Id);

        supplier.UpdateDetails(request.Name, request.Country, new ContactInfo(request.Email, request.Phone));
        supplierRepository.Update(supplier);

        return Result.Success();
    }
}