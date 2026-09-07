namespace TradeSphere.Persistence.Configurations;

public sealed class CustomsClearanceConfiguration : IEntityTypeConfiguration<CustomsClearance>
{
    public void Configure(EntityTypeBuilder<CustomsClearance> builder)
    {
        builder.ToTable("CustomsClearances");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.DeclarationNumber).HasMaxLength(100);
        builder.Property(c => c.Port).HasMaxLength(100);
        builder.Property(c => c.DeclaredGoodsValue).HasColumnType("decimal(18,2)");
        builder.Property(c => c.CustomsDuties).HasColumnType("decimal(18,2)");
        builder.Property(c => c.ClearanceFees).HasColumnType("decimal(18,2)");
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(20);
        builder.Ignore(c => c.TotalLandedCost); // computed in-memory, not stored

        builder.HasOne<Shipment>()
            .WithMany()
            .HasForeignKey(c => c.ShipmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}