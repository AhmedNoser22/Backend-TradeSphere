namespace TradeSphere.Application.Features.CustomsClearance.Clear;

public sealed record ClearCustomsCommand(Guid CustomsClearanceId, decimal CustomsDuties, decimal ClearanceFees) : IRequest<Result>, ITransactionalRequest;