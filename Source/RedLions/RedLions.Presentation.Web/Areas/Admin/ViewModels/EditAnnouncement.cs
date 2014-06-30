namespace RedLions.Presentation.Web.Areas.Admin.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DTO = RedLions.Application.DTO;
    using RedLions.Presentation.Web.Models;

    public class EditAnnouncement : Announcement
    {
        [Required]
        public override int ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }
    }
}