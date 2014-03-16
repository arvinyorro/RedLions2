namespace RedLions.Presentation.Web.Components
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Linq;

    public static class HtmlExtensions
    {
        public static MvcHtmlString EmptyLink(
            this HtmlHelper helper,
            string linkText)
        {
            var tag = new TagBuilder("a");
            tag.MergeAttribute("href", "javascript:void(0);");
            tag.SetInnerText(linkText);
            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString EmptyLink(
            this HtmlHelper helper,
            string linkText,
            object htmlAttributes)
        {
            var tag = new TagBuilder("a");
            tag.MergeAttribute("href", "javascript:void(0);");
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.SetInnerText(linkText);
            return MvcHtmlString.Create(tag.ToString());
        }

        public static MvcHtmlString HighlightActive(
            this HtmlHelper helper, 
            string action)
        {
            var currentAction = (string)helper.ViewContext.RouteData.Values["action"];
            if (string.Equals(currentAction,action, StringComparison.CurrentCultureIgnoreCase))
            {
                return MvcHtmlString.Create("class=\"active\"");
            }

            return MvcHtmlString.Empty;
        }
    }
}