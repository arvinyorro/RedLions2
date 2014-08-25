namespace RedLions.Presentation.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class PaymentGift
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public bool Checked { get; set; }
    }
}