namespace TradeSphere.Application.Features.CustomsClearance.Reject;

public sealed record RejectCustomsCommand(Guid CustomsClearanceId) : IRequest<Result>, ITransactionalRequest;