namespace RedLions.CrossCutting
{
    using System;

    public class UnitOfWork : IUnitOfWork
    {
        private IDbContext dbContext;

        public UnitOfWork(IDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            this.dbContext = dbContext;
        }

        public void Commit()
        {
            this.dbContext.SaveChanges();
        }
    }
}
