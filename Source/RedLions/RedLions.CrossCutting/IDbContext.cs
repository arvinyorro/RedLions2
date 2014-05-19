namespace RedLions.CrossCutting
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;

    public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbSet Set(Type entityType);

        int SaveChanges();

        void ExecuteSqlCommand(string query, params object[] parameters);

        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DbEntityEntry Entry(object entity);
    }
}
