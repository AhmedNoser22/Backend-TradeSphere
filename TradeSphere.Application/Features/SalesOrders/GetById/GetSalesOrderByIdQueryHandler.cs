namespace TradeSphere.Application.Features.SalesOrders.GetById;

public sealed class GetSalesOrderByIdQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetSalesOrderByIdQuery, SalesOrderDetailsDto>
{
    public async Task<SalesOrderDetailsDto> Handle(GetSalesOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = await dbContext.SalesOrders
            .Where(so => so.Id == request.SalesOrderId)
            .Select(so => new SalesOrderDetailsDto(
                so.Id, so.OrderNumber, so.CustomerId, so.Customer.Name, so.Status, so.OrderDate,
                so.Lines.Select(l => new SalesOrderLineDto(l.Id, l.ProductId, l.Product.Name, l.Quantity, l.UnitPrice)).ToList(),
                so.Lines.Sum(l => l.Quantity * l.UnitPrice)))
            .FirstOrDefaultAsync(cancellationToken);

        return dto ?? throw new NotFoundException(nameof(SalesOrder), request.SalesOrderId);
    }
}