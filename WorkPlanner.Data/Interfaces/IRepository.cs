namespace WorkPlanner.Data.Interfaces;

public interface IRepository
{
    IQueryable<TEntity> Set<TEntity>() where TEntity : class, IEntity;
    Task<TEntity?> Find<TEntity>(Guid id) where TEntity : class, IEntity;
    Task<TEntity?> Find<TEntity>(int id) where TEntity : class, IEntity;
    void Add<TEntity>(TEntity entity) where TEntity : IEntity;
    void Update<TEntity>(TEntity entity) where TEntity : IEntity;
    void Delete<TEntity>(TEntity entity) where TEntity : IEntity;
    void SoftDelete<TEntity>(TEntity entity) where TEntity : IEntity, ISoftDeletable;
    Task<int> SaveChangesAsync();
}