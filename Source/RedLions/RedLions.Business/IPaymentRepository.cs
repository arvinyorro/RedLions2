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
        IEnumerable<Payment> GetUnreadPayments();
        IEnumerable<Payment> GetUnreadPaymentsByMember(Member member);
        IEnumerable<Payment> GetPagedList(
            int pageIndex,
            int pageSize,
            out int totalCount,
            Expression<Func<Payment, bool>> filter = null);
        int GetUnreadCount(Expression<Func<Payment, bool>> filter);

        void Create(Payment payment);
        void Update(Payment payment);
    }
}
