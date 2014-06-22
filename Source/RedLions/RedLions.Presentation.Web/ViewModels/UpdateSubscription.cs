namespace RedLions.Presentation.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using RedLions.Presentation.Web.Components;
    using RedLions.Presentation.Web.Models;
    using DTO = RedLions.Application.DTO;

    public class UpdateSubscription
    {
        public UpdateSubscription(
            DTO.Member memberDTO,
            IEnumerable<IDropDown> subscriptionDropDownItems)
        {
            this.FullName = string.Format("{0} {1}", memberDTO.FirstName, memberDTO.LastName);
            this.UserID = memberDTO.ID;
            this.SubscriptionID = memberDTO.Subscription.ID;
            this.SubscriptionSelectListItems = subscriptionDropDownItems.ToSelectListItems(memberDTO.Country.ID);
        }

        public string FullName { get; set; }

        public int UserID { get; set; }

        [Display(Name="New Subscription Plan")]
        public int SubscriptionID { get; set; }

        public IEnumerable<SelectListItem> SubscriptionSelectListItems { get; set; }
    }
}