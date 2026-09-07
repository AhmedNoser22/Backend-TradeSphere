namespace TradeSphere.Persistence.Configurations;

public sealed class QualityInspectionConfiguration : IEntityTypeConfiguration<QualityInspection>
{
    public void Configure(EntityTypeBuilder<QualityInspection> builder)
    {
        builder.ToTable("QualityInspections");
        builder.HasKey(q => q.Id);

        builder.Property(q => q.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasMany(q => q.Lines)
            .WithOne()
            .HasForeignKey(l => l.QualityInspectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(q => q.DomainEvents);
    }
}

public sealed class QualityInspectionLineConfiguration : IEntityTypeConfiguration<QualityInspectionLine>
{
    public void Configure(EntityTypeBuilder<QualityInspectionLine> builder)
    {
        builder.ToTable("QualityInspectionLines");
        builder.HasKey(l => l.Id);

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(l => l.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}