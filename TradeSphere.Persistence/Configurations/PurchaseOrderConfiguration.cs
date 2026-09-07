namespace TradeSphere.Persistence.Configurations;

public sealed class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("PurchaseOrders");
        builder.HasKey(po => po.Id);

        builder.Property(po => po.OrderNumber).HasMaxLength(50).IsRequired();
        builder.HasIndex(po => po.OrderNumber).IsUnique();
        builder.Property(po => po.Status).HasConversion<string>().HasMaxLength(30);

        // Lines belong to the order (aggregate) — cascade delete is correct here
        // because a PurchaseOrderLine has no meaning without its parent order.
        builder.HasMany(po => po.Lines)
            .WithOne()
            .HasForeignKey(l => l.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Domain events are in-memory only, never persisted as a column.
        builder.Ignore(po => po.DomainEvents);
    }
}