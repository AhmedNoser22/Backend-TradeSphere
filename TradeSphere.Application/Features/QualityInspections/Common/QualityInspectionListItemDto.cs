namespace TradeSphere.Application.Features.QualityInspections.Common;

public sealed record QualityInspectionListItemDto(
    Guid Id, Guid CustomsClearanceId, Guid PurchaseOrderId, QualityInspectionStatus Status, DateTimeOffset? CompletedAtUtc);

public sealed record QualityInspectionLineDto(Guid Id, Guid ProductId, string ProductName, int AcceptedQuantity, int RejectedQuantity, int MissingQuantity);

public sealed record QualityInspectionDetailsDto(
    Guid Id, Guid CustomsClearanceId, Guid PurchaseOrderId, QualityInspectionStatus Status,
    DateTimeOffset? CompletedAtUtc, List<QualityInspectionLineDto> Lines);