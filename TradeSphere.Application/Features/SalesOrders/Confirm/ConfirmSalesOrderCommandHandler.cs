namespace TradeSphere.Application.Features.SalesOrders.Confirm;

public sealed class ConfirmSalesOrderCommandHandler(IRepository<SalesOrder> salesOrderRepository) : IRequestHandler<ConfirmSalesOrderCommand, Result>
{
    public async Task<Result> Handle(ConfirmSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var salesOrder = await salesOrderRepository.FirstOrDefaultAsync(
            new SalesOrderByIdWithLinesSpecification(request.SalesOrderId), cancellationToken)
            ?? throw new NotFoundException(nameof(SalesOrder), request.SalesOrderId);

        try
        {
            salesOrder.Confirm();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }

        salesOrderRepository.Update(salesOrder);
        return Result.Success();
    }
}