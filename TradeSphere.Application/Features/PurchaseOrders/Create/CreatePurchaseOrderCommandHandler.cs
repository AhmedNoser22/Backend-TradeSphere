namespace TradeSphere.Application.Features.PurchaseOrders.Create;

public sealed class CreatePurchaseOrderCommandHandler(
    IRepository<PurchaseOrder> purchaseOrderRepository,
    IRepository<Supplier> supplierRepository,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<CreatePurchaseOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken)
            ?? throw new NotFoundException(nameof(Supplier), request.SupplierId);

        if (!supplier.IsActive)
            return Result<Guid>.Failure("Cannot create a purchase order for an inactive supplier.");

        var orderNumber = $"PO-{dateTimeProvider.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";

        var purchaseOrder = new PurchaseOrder(orderNumber, request.SupplierId, request.OrderDate);
        await purchaseOrderRepository.AddAsync(purchaseOrder, cancellationToken);

        return Result<Guid>.Success(purchaseOrder.Id);
    }
}