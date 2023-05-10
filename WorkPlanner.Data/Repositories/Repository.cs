using WorkPlanner.Data.Interfaces;

namespace WorkPlanner.Data.Repositories;

public class Repository : IRepository
{
    private readonly AppDbContext _dbContext;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> Set<TEntity>() where TEntity : class, IEntity
    {
        return _dbContext.Set<TEntity>();
    }

    public Task<TEntity?> Find<TEntity>(Guid id) where TEntity : class, IEntity
    {
        return Find<TEntity, Guid>(id);
    }

    public Task<TEntity?> Find<TEntity>(int id) where TEntity : class, IEntity
    {
        return Find<TEntity, int>(id);
    }

    public void Add<TEntity>(TEntity entity) where TEntity : IEntity
    {
        _dbContext.Add(entity);
    }

    public void Update<TEntity>(TEntity entity) where TEntity : IEntity
    {
        _dbContext.Update(entity);
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : IEntity
    {
        _dbContext.Remove(entity);
    }

    public void SoftDelete<TEntity>(TEntity entity) where TEntity : IEntity, ISoftDeletable
    {
        entity.Deleted = DateTime.Now;
    }

    public Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }

    private async Task<TEntity?> Find<TEntity, TId>(TId id) where TEntity : class, IEntity where TId : IEquatable<TId>
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }
}