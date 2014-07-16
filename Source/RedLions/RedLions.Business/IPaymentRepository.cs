namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPaymentRepository
    {
        Payment GetByPublicID(string publicID);
        Payment GetByID(int id);
        IEnumerable<Payment> GetPagedList(
            int pageIndex,
            int pageSize,
            out int totalCount);

        void Create(Payment payment);
        void Update(Payment payment);
    }
}
