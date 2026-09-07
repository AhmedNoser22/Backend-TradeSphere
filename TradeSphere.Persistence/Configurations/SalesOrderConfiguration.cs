namespace TradeSphere.Persistence.Configurations;

public sealed class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder.ToTable("SalesOrders");
        builder.HasKey(so => so.Id);

        builder.Property(so => so.OrderNumber).HasMaxLength(50).IsRequired();
        builder.HasIndex(so => so.OrderNumber).IsUnique();
        builder.Property(so => so.Status).HasConversion<string>().HasMaxLength(20);
        builder.Ignore(so => so.TotalAmount); // computed from lines, not stored
        builder.Ignore(so => so.DomainEvents);

        builder.HasMany(so => so.Lines)
            .WithOne()
            .HasForeignKey(l => l.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class SalesOrderLineConfiguration : IEntityTypeConfiguration<SalesOrderLine>
{
    public void Configure(EntityTypeBuilder<SalesOrderLine> builder)
    {
        builder.ToTable("SalesOrderLines");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.UnitPrice).HasColumnType("decimal(18,2)");

        builder.HasOne(l => l.Product)
            .WithMany()
            .HasForeignKey(l => l.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}