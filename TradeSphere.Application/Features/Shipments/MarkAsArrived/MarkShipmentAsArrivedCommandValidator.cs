namespace TradeSphere.Application.Features.Shipments.MarkAsArrived;
public sealed class MarkShipmentAsArrivedCommandValidator : AbstractValidator<MarkShipmentAsArrivedCommand>
{
    public MarkShipmentAsArrivedCommandValidator() => RuleFor(x => x.ShipmentId).NotEmpty();
}