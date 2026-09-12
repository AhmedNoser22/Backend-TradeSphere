namespace TradeSphere.Persistence.Repositories;
public sealed class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : BaseEntity
{
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await context.Set<T>().FindAsync([id], cancellationToken);

    public async Task<bool> AnyAsync(ISpecification<T> spec, CancellationToken cancellationToken = default) =>
    await context.Set<T>().Where(spec.Criteria).AnyAsync(cancellationToken);

    public async Task<T?> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default) =>
        await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);

    public async Task<List<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default) =>
        await ApplySpecification(spec).ToListAsync(cancellationToken);

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await context.Set<T>().AddAsync(entity, cancellationToken);
        return entity;
    }

    public void Update(T entity)
    {
        if (context.Entry(entity).State == EntityState.Detached)
        {
            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }

    public void Remove(T entity) => context.Set<T>().Remove(entity);

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        IQueryable<T> query = context.Set<T>();
        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
        return query.Where(spec.Criteria);
    }
}