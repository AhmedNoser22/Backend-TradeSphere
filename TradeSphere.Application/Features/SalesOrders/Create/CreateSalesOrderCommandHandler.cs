namespace TradeSphere.Application.Features.SalesOrders.Create;

public sealed class CreateSalesOrderCommandHandler(
    IRepository<SalesOrder> salesOrderRepository,
    IRepository<Customer> customerRepository,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<CreateSalesOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        if (!customer.IsActive)
            return Result<Guid>.Failure("Cannot create a sales order for an inactive customer.");

        var orderNumber = $"SO-{dateTimeProvider.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";

        var salesOrder = new SalesOrder(orderNumber, request.CustomerId, request.OrderDate);
        await salesOrderRepository.AddAsync(salesOrder, cancellationToken);

        return Result<Guid>.Success(salesOrder.Id);
    }
}