namespace RedLions.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using RedLions.Business;
    using RedLions.CrossCutting;

    public class PaymentRepository : GenericRepository, IPaymentRepository
    {
        public PaymentRepository(IDbContext dbContext)
            : base(dbContext) { }

        
        public Payment GetByPublicID(string publicID)
        {
            return base.GetSingle<Payment>(x => x.PublicID == publicID);
        }

        public Payment GetByID(int id)
        {
            return base.GetById<Payment>(id);
        }

        public IEnumerable<Payment> GetUnreadPayments()
        {
            return base.GetAll<Payment>().Where(x => x.AdminUnread == true);
        }

        public IEnumerable<Payment> GetUnreadPaymentsByMember(Member member)
        {
            return base.GetAll<Payment>()
                .Where(x =>
                    x.ReferrerUnread == true &&
                    x.Referrer.ID == member.ID);
        }

        public IEnumerable<Payment> GetPagedList(
            int pageIndex, 
            int pageSize, 
            out int totalCount,
            Expression<Func<Payment, bool>> filter = null)
        {
            var query = from x in base.GetQueryableAll<Payment>()
                        select x;

            totalCount = query.Count();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var orderedQuery = query.OrderByDescending(x => x.CreatedDateTime);

            IEnumerable<Payment> payments = base.GetPagedList<Payment>(orderedQuery, pageIndex, pageSize);

            return payments;
        }

        public int GetUnreadCount(Expression<Func<Payment, bool>> filter)
        {
            return base.Count<Payment>(filter);
        }

        public void Create(Payment payment)
        {
            base.Create<Payment>(payment);
        }

        public void Update(Payment payment)
        {
            base.Update<Payment>(payment);
        }
    }
}
