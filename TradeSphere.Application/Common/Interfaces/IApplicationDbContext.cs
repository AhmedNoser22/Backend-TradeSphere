namespace TradeSphere.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<Supplier> Suppliers { get; }
    DbSet<Product> Products { get; }
    DbSet<PurchaseOrder> PurchaseOrders { get; }
    DbSet<Shipment> Shipments { get; }
    DbSet<CustomsClearance> CustomsClearances { get; }
    DbSet<QualityInspection> QualityInspections { get; }
    DbSet<InventoryStock> InventoryStocks { get; }
    DbSet<Customer> Customers { get; }
    DbSet<SalesOrder> SalesOrders { get; }
    DbSet<Payment> Payments { get; }
    DbSet<User> Users { get; }

    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}