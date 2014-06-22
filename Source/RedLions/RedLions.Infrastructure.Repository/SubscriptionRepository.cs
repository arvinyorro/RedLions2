namespace RedLions.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RedLions.Business;
    using RedLions.CrossCutting;

    public class SubscriptionRepository : GenericRepository, ISubscriptionRepository
    {
        public SubscriptionRepository(IDbContext dbContext)
            : base(dbContext) { }

        public Subscription GetByID(int id)
        {
            return base.GetById<Subscription>(id);
        }

        public IEnumerable<Subscription> GetAll()
        {
            return base.GetAll<Subscription>();
        }
    }
}
