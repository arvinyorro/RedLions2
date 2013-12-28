namespace RedLions.Infrastructure.Repository
{
    using System;
    using System.Data.Entity.Validation;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using RedLions.Business;

    public class InquiryRepository : GenericRepository, IInquiryRepository
    {
        public InquiryRepository(RedLionsContext context)
            : base(context)
        {
        }

        public void Inquire(Inquiry inquiry)
        {
            if (inquiry == null)
            {
                throw new ArgumentNullException("The parameter 'inquiry' must not be null.");
            }

            base.Create<Inquiry>(inquiry);
        }

        public Inquiry GetById(int id)
        {
            return base.GetById<Inquiry>(id);
        }

        public IEnumerable<Inquiry> GetAllInquiries(bool registered)
        {
            return base.GetAll<Inquiry>(x => x.Registered == registered);
        }

        public IEnumerable<Inquiry> GetPagedList<TKey>(
            int pageIndex,
            int pageSize,
            out int totalCount,
            Expression<Func<Inquiry, TKey>> order,
            Expression<Func<Inquiry, bool>> filter = null)
        {
            return base.GetPagedList<Inquiry, TKey>(pageIndex, pageSize, out totalCount, order, filter);
        }

        public int Count(bool registered)
        {
            return base.Count<Inquiry>(x => x.Registered == registered);
        }
    }
}
