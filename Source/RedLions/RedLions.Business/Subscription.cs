namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Subscription
    {
        public int ID { get; private set; }
        public string Title { get; set; }
        public int Months { get; set; }
    }
}
