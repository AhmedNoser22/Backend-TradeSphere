namespace TradeSphere.Application.Features.Dashboard.GetSummary;
public sealed class GetDashboardSummaryQueryHandler(
    IApplicationDbContext dbContext,
    IRepository<InventoryStock> inventoryRepository,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        var totalSalesAmount = await dbContext.SalesOrders
            .Where(so => so.Status == SalesOrderStatus.Delivered)
            .SelectMany(so => so.Lines)
            .SumAsync(l => (decimal?)(l.Quantity * l.UnitPrice), cancellationToken) ?? 0m;

        var totalPurchasesAmount = await dbContext.PurchaseOrders
            .Where(po => po.Status != PurchaseOrderStatus.Draft && po.Status != PurchaseOrderStatus.Cancelled)
            .SelectMany(po => po.Lines)
            .SumAsync(l => (decimal?)(l.Quantity * l.UnitPrice), cancellationToken) ?? 0m;

        // Approximate valuation: quantity on hand times the average purchase
        // unit price ever recorded for that product — not FIFO/actual cost,
        // a deliberate simplification for reporting purposes.
        var totalInventoryValue = await dbContext.InventoryStocks
            .Select(s => new
            {
                s.QuantityOnHand,
                AverageUnitPrice = dbContext.Set<PurchaseOrderLine>()
                    .Where(l => l.ProductId == s.ProductId)
                    .Average(l => (decimal?)l.UnitPrice) ?? 0m
            })
            .SumAsync(x => x.QuantityOnHand * x.AverageUnitPrice, cancellationToken);

        var totalReceivables = await dbContext.Payments
            .Where(p => p.Direction == PaymentDirection.Incoming && p.Status != PaymentStatus.FullyPaid)
            .SumAsync(p => (decimal?)(p.TotalDue - p.AmountPaid), cancellationToken) ?? 0m;

        var totalPayables = await dbContext.Payments
            .Where(p => p.Direction == PaymentDirection.Outgoing && p.Status != PaymentStatus.FullyPaid)
            .SumAsync(p => (decimal?)(p.TotalDue - p.AmountPaid), cancellationToken) ?? 0m;

        var lowStockStocks = await inventoryRepository.ListAsync(new LowStockProductsSpecification(request.LowStockThreshold), cancellationToken);
        var lowStockProducts = lowStockStocks
            .Select(s => new InventoryStockListItemDto(s.ProductId, s.Product.Sku, s.Product.Name, s.QuantityOnHand))
            .OrderBy(dto => dto.QuantityOnHand)
            .ToList();

        var delayCutoff = dateTimeProvider.UtcNow.AddDays(-request.CustomsDelayDays);
        var delayedRaw = await dbContext.CustomsClearances
            .Where(c => (c.Status == CustomsClearanceStatus.PendingDocuments || c.Status == CustomsClearanceStatus.UnderClearance)
                        && c.CreatedAtUtc <= delayCutoff)
            .OrderBy(c => c.CreatedAtUtc)
            .Take(20)
            .Select(c => new { c.Id, c.ShipmentId, c.Status, c.CreatedAtUtc })
            .ToListAsync(cancellationToken);

        var delayedCustomsClearances = delayedRaw
            .Select(c => new DelayedCustomsClearanceDto(c.Id, c.ShipmentId, c.Status, c.CreatedAtUtc, (dateTimeProvider.UtcNow - c.CreatedAtUtc).Days))
            .ToList();

        return new DashboardSummaryDto(
            totalSalesAmount, totalPurchasesAmount, totalInventoryValue,
            totalReceivables, totalPayables, lowStockProducts, delayedCustomsClearances);
    }
}