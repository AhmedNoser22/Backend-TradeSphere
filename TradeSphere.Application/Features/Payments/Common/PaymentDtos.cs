namespace TradeSphere.Application.Features.Payments.Common;

public sealed record PaymentListItemDto(
    Guid Id, PaymentDirection Direction, Guid ReferenceOrderId, string OrderNumber, string PartyName,
    decimal TotalDue, decimal AmountPaid, decimal RemainingBalance, string Currency, PaymentStatus Status);