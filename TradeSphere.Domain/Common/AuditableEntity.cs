namespace TradeSphere.Domain.Common;
public class AuditableEntity : BaseEntity
{
    public DateTimeOffset CreatedAtUtc { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTimeOffset? LastModifiedAtUtc { get; set; }
    public Guid? LastModifiedByUserId { get; set; }
}