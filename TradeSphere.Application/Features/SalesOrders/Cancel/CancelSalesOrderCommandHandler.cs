namespace TradeSphere.Application.Features.SalesOrders.Cancel;

public sealed class CancelSalesOrderCommandHandler(IRepository<SalesOrder> salesOrderRepository) : IRequestHandler<CancelSalesOrderCommand, Result>
{
    public async Task<Result> Handle(CancelSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var salesOrder = await salesOrderRepository.GetByIdAsync(request.SalesOrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrder), request.SalesOrderId);

        try
        {
            salesOrder.Cancel();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }

        salesOrderRepository.Update(salesOrder);
        return Result.Success();
    }
}