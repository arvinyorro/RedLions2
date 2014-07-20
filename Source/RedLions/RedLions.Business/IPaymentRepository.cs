namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Linq.Expressions;

    public interface IPaymentRepository
    {
        Payment GetByPublicID(string publicID);
        Payment GetByID(int id);
        IEnumerable<Payment> GetPagedList(
            int pageIndex,
            int pageSize,
            out int totalCount,
            Expression<Func<Payment, bool>> filter = null);

        void Create(Payment payment);
        void Update(Payment payment);
    }
}
