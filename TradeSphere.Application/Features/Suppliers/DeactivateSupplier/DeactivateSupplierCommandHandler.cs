namespace TradeSphere.Application.Features.Suppliers.DeactivateSupplier;

public sealed class DeactivateSupplierCommandHandler(IRepository<Supplier> supplierRepository) : IRequestHandler<DeactivateSupplierCommand, Result>
{
    public async Task<Result> Handle(DeactivateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Supplier), request.Id);

        supplier.Deactivate();
        supplierRepository.Update(supplier);

        return Result.Success();
    }
}