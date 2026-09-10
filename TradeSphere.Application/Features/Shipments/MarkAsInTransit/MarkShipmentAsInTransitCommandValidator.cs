namespace TradeSphere.Application.Features.Shipments.MarkAsInTransit;

public sealed class MarkShipmentAsInTransitCommandValidator : AbstractValidator<MarkShipmentAsInTransitCommand>
{
    public MarkShipmentAsInTransitCommandValidator() => RuleFor(x => x.ShipmentId).NotEmpty();
}