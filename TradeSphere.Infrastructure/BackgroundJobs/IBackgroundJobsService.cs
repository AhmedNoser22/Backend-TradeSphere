namespace TradeSphere.Infrastructure.BackgroundJobs;
public interface IBackgroundJobsService
{
    void Enqueue(Expression<Action> methodCall);
}