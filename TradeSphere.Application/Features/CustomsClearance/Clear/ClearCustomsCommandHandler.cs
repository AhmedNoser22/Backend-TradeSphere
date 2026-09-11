namespace TradeSphere.Application.Features.CustomsClearance.Clear;

public sealed class ClearCustomsCommandHandler(IRepository<Domain.Entities.CustomsClearance> customsRepository)
    : IRequestHandler<ClearCustomsCommand, Result>
{
    public async Task<Result> Handle(ClearCustomsCommand request, CancellationToken cancellationToken)
    {
        var clearance = await customsRepository.GetByIdAsync(request.CustomsClearanceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.CustomsClearance), request.CustomsClearanceId);

        try
        {
            clearance.Clear(request.CustomsDuties, request.ClearanceFees);
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }

        customsRepository.Update(clearance);
        return Result.Success();
    }
}