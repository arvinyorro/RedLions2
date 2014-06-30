namespace RedLions.Presentation.Web.ViewModels
{
    using System;
    using System.Web;
    using RedLions.Presentation.Web.Models;

    public class PublicAnnouncement : Announcement
    {
        public PublicAnnouncement()
        {
            var request = HttpContext.Current.Request;
            this.ShareLink = string.Format("{0}",request.Url);
        }

        public string ShareLink { get; set; }
    }
}