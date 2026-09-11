//public sealed class QualityInspectionCompletedEventHandler(IApplicationDbContext dbContext)
//    : INotificationHandler<DomainEventNotification<QualityInspectionCompletedEvent>>
//{
//    public async Task Handle(DomainEventNotification<QualityInspectionCompletedEvent> notification, CancellationToken cancellationToken)
//    {
//        var domainEvent = notification.DomainEvent;

//        foreach (var line in domainEvent.AcceptedLines)
//        {
//            var stock = await dbContext.InventoryStocks.FirstOrDefaultAsync(s => s.ProductId == line.ProductId, cancellationToken);

//            if (stock is null)
//            {
//                stock = new InventoryStock(line.ProductId);
//                await dbContext.InventoryStocks.AddAsync(stock, cancellationToken);
//            }

//            stock.Receive(line.AcceptedQuantity, domainEvent.InspectionId, "Received from completed quality inspection");
//        }

//        var purchaseOrder = await dbContext.PurchaseOrders.FirstOrDefaultAsync(po => po.Id == domainEvent.PurchaseOrderId, cancellationToken);

//        if (purchaseOrder is { Status: PurchaseOrderStatus.FullyShipped })
//            purchaseOrder.Close();

//        await dbContext.SaveChangesAsync(cancellationToken);
//    }
//}