namespace RedLions.Presentation.Web.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using RedLions.Presentation.Web.Models;

    /// <summary>
    /// Contains collections of custom method extensions for enumerables.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// This routine converts an enumerable, that inherits the <see cref="Models.IDropDown"/>
        /// interface, into an enumerable of <see cref="SelectListItem"/>.
        /// </summary>
        /// <param name="list">List of enumerable to convert.</param>
        /// <param name="selectedValue">Default selected option in the <see cref="SelectListItem"/>.</param>
        /// <returns>An enumerable of <see cref="SelectListItem"/>.</returns>
        public static IEnumerable<SelectListItem> ToSelectListItems(
            this IEnumerable<IDropDown> list,
            int selectedValue = 0)
        {
            var selectItems = list.Select(x => new SelectListItem
            {
                Value = x.ID.ToString(),
                Text = x.Title,
                Selected = x.ID == selectedValue
            });

            return selectItems;
        }
    }
}