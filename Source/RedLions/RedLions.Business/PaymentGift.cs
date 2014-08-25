namespace RedLions.Business
{
    using System;

    public class PaymentGift
    {
        protected PaymentGift()
        {
            // Required by EF.
        }

        public PaymentGift(string title, decimal price)
        {
            this.Title = title;
            this.Price = price;
        }

        public int ID { get; private set; }

        public string Title { get; private set; }

        public decimal Price { get; private set; }

        public virtual Payment Payment { get; private set; }
    }
}
