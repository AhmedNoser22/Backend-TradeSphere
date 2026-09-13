namespace TradeSphere.Infrastructure.BackgroundJobs;
public sealed class ScheduledJobs(IApplicationDbContext dbContext, IEmailService emailService, IDateTimeProvider dateTimeProvider)
{
    private const int DelayThresholdDays = 5;
    private const int LowStockThreshold = 10;

    public async Task CheckDelayedCustomsClearancesAsync()
    {
        var cutoff = dateTimeProvider.UtcNow.AddDays(-DelayThresholdDays);

        var delayedCount = await dbContext.CustomsClearances.CountAsync(c =>
            (c.Status == CustomsClearanceStatus.PendingDocuments || c.Status == CustomsClearanceStatus.UnderClearance)
            && c.CreatedAtUtc <= cutoff);

        if (delayedCount == 0)
            return;

        var recipients = await dbContext.Users
            .Where(u => u.IsActive && (u.Role == UserRole.OperationsManager || u.Role == UserRole.CustomsClearanceOfficer))
            .Select(u => u.Email)
            .ToListAsync();

        var body = $"<p>{delayedCount} customs clearance case(s) have been pending for more than {DelayThresholdDays} days.</p>";

        foreach (var email in recipients)
            await emailService.SendAsync(email, "TradeSphere: Delayed customs clearances", body);
    }

    public async Task CheckLowStockAsync()
    {
        var lowStockProducts = await dbContext.InventoryStocks
            .Where(s => s.QuantityOnHand <= LowStockThreshold)
            .Select(s => new { s.Product.Name, s.QuantityOnHand })
            .ToListAsync();

        if (lowStockProducts.Count == 0)
            return;

        var recipients = await dbContext.Users
            .Where(u => u.IsActive && (u.Role == UserRole.OperationsManager || u.Role == UserRole.WarehouseOfficer))
            .Select(u => u.Email)
            .ToListAsync();

        var listHtml = string.Join("", lowStockProducts.Select(p => $"<li>{p.Name}: {p.QuantityOnHand}</li>"));
        var body = $"<p>The following products are low on stock:</p><ul>{listHtml}</ul>";

        foreach (var email in recipients)
            await emailService.SendAsync(email, "TradeSphere: Low stock alert", body);
    }
}