using APM.Domain.Entities;
using APM.Domain.Interfaces;
using APM.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace APM.Infrastructure
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        IQueryable<Product> IApplicationDbContext.Products => Products;

        public new void Add<TEntity>(TEntity entity) where TEntity : class
        {
            base.Add(entity);
        }

        public new void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            base.Remove(entity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Automatically set UpdatedAt timestamp
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Product && 
                           (e.State == EntityState.Modified || e.State == EntityState.Added));

            foreach (var entityEntry in entries)
            {
                var product = (Product)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    product.CreatedAt = DateTime.UtcNow;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    product.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}

