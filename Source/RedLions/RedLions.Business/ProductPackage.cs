namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;

    public class ProductPackage
    {
        protected ProductPackage()
        {

        }

        public int ID { get; private set; }

        public string Title { get; private set; }

        public DateTime CreatedDateTime { get; private set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}