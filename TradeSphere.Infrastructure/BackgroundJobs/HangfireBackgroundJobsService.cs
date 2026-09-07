namespace TradeSphere.Infrastructure.BackgroundJobs;

public sealed class HangfireBackgroundJobsService : IBackgroundJobsService
{
    public void Enqueue(Expression<Action> methodCall) => BackgroundJob.Enqueue(methodCall);
}