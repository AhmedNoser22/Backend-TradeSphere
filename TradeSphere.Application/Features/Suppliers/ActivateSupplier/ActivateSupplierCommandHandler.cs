namespace TradeSphere.Application.Features.Suppliers.ActivateSupplier;

public sealed class ActivateSupplierCommandHandler(IRepository<Supplier> supplierRepository) : IRequestHandler<ActivateSupplierCommand, Result>
{
    public async Task<Result> Handle(ActivateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Supplier), request.Id);

        supplier.Activate();
        supplierRepository.Update(supplier);

        return Result.Success();
    }
}