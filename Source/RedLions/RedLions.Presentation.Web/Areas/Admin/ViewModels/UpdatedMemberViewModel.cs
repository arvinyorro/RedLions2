namespace RedLions.Presentation.Web.Areas.Admin.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using RedLions.Presentation.Web.Models;
    using RedLions.Presentation.Web.Components;

    public class UpdatedMemberViewModel
    {
        public UpdatedMemberViewModel(
            Models.Member member,
            IEnumerable<IDropDown> countryDropDownItems)
        {
            this.Member = member;
            this.CountrySelectListItems = countryDropDownItems.ToSelectListItems(member.Country.ID);
        }

        public Member Member { get; set; }
        public IEnumerable<SelectListItem> CountrySelectListItems { get; set; }
    }
}