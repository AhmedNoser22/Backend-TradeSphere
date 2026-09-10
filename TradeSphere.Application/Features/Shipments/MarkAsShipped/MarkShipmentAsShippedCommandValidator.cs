namespace TradeSphere.Application.Features.Shipments.MarkAsShipped;

public sealed class MarkShipmentAsShippedCommandValidator : AbstractValidator<MarkShipmentAsShippedCommand>
{
    public MarkShipmentAsShippedCommandValidator() => RuleFor(x => x.ShipmentId).NotEmpty();
}