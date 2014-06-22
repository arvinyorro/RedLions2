namespace RedLions.Application.DTO
{
    using System.Collections.Generic;
    using System.Linq;

    public class Subscription
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Months { get; set; }
    }

    internal static class SubscriptionAssembler
    {
        internal static DTO.Subscription ToDTO(this Business.Subscription subscription)
        {
            var subscriptionDTO = new DTO.Subscription()
            {
                ID = subscription.ID,
                Title = subscription.Title,
                Months = subscription.Months,
            };

            return subscriptionDTO;
        }
    }
}
