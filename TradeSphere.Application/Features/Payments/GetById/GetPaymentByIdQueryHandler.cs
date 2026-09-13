namespace TradeSphere.Application.Features.Payments.GetById;

public sealed class GetPaymentByIdQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetPaymentByIdQuery, PaymentListItemDto>
{
    public async Task<PaymentListItemDto> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await dbContext.Payments.FirstOrDefaultAsync(p => p.Id == request.PaymentId, cancellationToken)
            ?? throw new NotFoundException(nameof(Payment), request.PaymentId);

        var dto = payment.Direction == PaymentDirection.Incoming
            ? await dbContext.SalesOrders.Where(so => so.Id == payment.ReferenceOrderId)
                .Select(so => new PaymentListItemDto(payment.Id, payment.Direction, payment.ReferenceOrderId, so.OrderNumber, so.Customer.Name,
                    payment.TotalDue, payment.AmountPaid, payment.RemainingBalance, payment.Currency, payment.Status))
                .FirstOrDefaultAsync(cancellationToken)
            : await dbContext.PurchaseOrders.Where(po => po.Id == payment.ReferenceOrderId)
                .Select(po => new PaymentListItemDto(payment.Id, payment.Direction, payment.ReferenceOrderId, po.OrderNumber, po.Supplier.Name,
                    payment.TotalDue, payment.AmountPaid, payment.RemainingBalance, payment.Currency, payment.Status))
                .FirstOrDefaultAsync(cancellationToken);

        return dto ?? throw new NotFoundException(nameof(Payment), request.PaymentId);
    }
}