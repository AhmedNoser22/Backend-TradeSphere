namespace TradeSphere.Application.Features.PurchaseOrders.GetById;

public sealed class GetPurchaseOrderByIdQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetPurchaseOrderByIdQuery, PurchaseOrderDetailsDto>
{
    public async Task<PurchaseOrderDetailsDto> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = await dbContext.PurchaseOrders
            .Where(po => po.Id == request.PurchaseOrderId)
            .Select(po => new PurchaseOrderDetailsDto(
                po.Id, po.OrderNumber, po.SupplierId, po.Supplier.Name, po.Status, po.OrderDate,
                po.Lines.Select(l => new PurchaseOrderLineDto(l.Id, l.ProductId, l.Product.Name, l.Quantity, l.UnitPrice, l.Currency)).ToList(),
                po.Lines.Sum(l => l.Quantity * l.UnitPrice)))
            .FirstOrDefaultAsync(cancellationToken);

        return dto ?? throw new NotFoundException(nameof(PurchaseOrder), request.PurchaseOrderId);
    }
}