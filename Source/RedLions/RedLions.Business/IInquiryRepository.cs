namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IInquiryRepository
    {
        void Inquire(Inquiry inquiry);
        Inquiry GetById(int id);
        IEnumerable<Inquiry> GetAllInquiries(bool registered);
        IEnumerable<Inquiry> GetPagedList<TKey>(
            int pageIndex,
            int pageSize,
            out int totalCount,
            Expression<Func<Inquiry, TKey>> order,
            Expression<Func<Inquiry, bool>> filter = null);
        int Count(bool registered);
    }
}
