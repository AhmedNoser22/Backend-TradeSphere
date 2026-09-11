namespace TradeSphere.Application.Features.CustomsClearance.Reject;

public sealed class RejectCustomsCommandHandler(IRepository<Domain.Entities.CustomsClearance> customsRepository)
    : IRequestHandler<RejectCustomsCommand, Result>
{
    public async Task<Result> Handle(RejectCustomsCommand request, CancellationToken cancellationToken)
    {
        var clearance = await customsRepository.GetByIdAsync(request.CustomsClearanceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.CustomsClearance), request.CustomsClearanceId);

        try
        {
            clearance.Reject();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }

        customsRepository.Update(clearance);
        return Result.Success();
    }
}