namespace RedLions.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using RedLions.CrossCutting;

    /// <summary>
    /// This class implements the <see cref="CrossCutting.IRepository"/> interface.
    /// </summary>
    public class GenericRepository : IRepository
    {
        private IDbContext context;
        public GenericRepository(IDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context must not be null.");
            }
            this.context = context;
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return context.Set<TEntity>().ToList();
        }

        public TEntity GetById<TEntity>(int id) where TEntity : class
        {
            return this.context.Set<TEntity>().Find(id);
        }

        public TEntity GetSingle<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            return this.context.Set<TEntity>().FirstOrDefault(filter);
        }

        public IEnumerable<TEntity> GetPagedList<TEntity>(int pageIndex, int pageSize) where TEntity : class
        {
            return this.context.Set<TEntity>().Skip(pageSize * pageIndex).Take(pageSize).ToList();
        }

        public IEnumerable<TEntity> GetPagedList<TEntity, TKey>(
            int pageIndex,
            int pageSize,
            out int totalCount,
            Expression<Func<TEntity, TKey>> order,
            Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            var query = from x in this.context.Set<TEntity>()
                        select x;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            totalCount = query.Count();

            // Warning: OrderBy() must be used before Skip();
            query = query.OrderBy(order);


            pageIndex = (pageIndex <= 0 ? 1 : pageIndex) - 1;
            query = query.Skip(pageSize * pageIndex).Take(pageSize);

            return query.ToList();
        }

        public IEnumerable<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            return this.context.Set<TEntity>().Where(filter).ToList();
        }

        public int Count<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            return this.context.Set<TEntity>().Count(filter);
        }

        public int Count<TEntity>() where TEntity : class
        {
            return this.context.Set<TEntity>().Count();
        }

        public void Create<TEntity>(TEntity entity) where TEntity : class
        {
            this.context.Set<TEntity>().Add(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            this.context.Entry<TEntity>(entity).State = System.Data.Entity.EntityState.Modified;
            this.Save();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            this.context.Set<TEntity>().Remove(entity);
            this.context.SaveChanges();
        }

        private void Save()
        {
            try
            {
                this.context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }
    }
}
