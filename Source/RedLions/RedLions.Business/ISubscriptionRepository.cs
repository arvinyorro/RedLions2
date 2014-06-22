using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedLions.Business
{
    public interface ISubscriptionRepository
    {
        Subscription GetByID(int id);
        IEnumerable<Subscription> GetAll();
    }
}
