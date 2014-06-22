namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Subscription
    {
        protected Subscription()
        {
            // Required by EF.
        }

        public Subscription(string title, int months)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            this.Title = title;

            if (months == 0)
            {
                throw new Exception("Subscription month must not be 0");
            }

            this.Months = months;
        }

        public int ID { get; private set; }
        public string Title { get; private set; }
        public int Months { get; private set; }
    }
}
