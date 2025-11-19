using APM.Domain.Entities;

namespace APM.Domain.Interfaces
{
    public interface IApplicationDbContext
    {
        IQueryable<Product> Products { get; }
        void Add<TEntity>(TEntity entity) where TEntity : class;
        void Remove<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
