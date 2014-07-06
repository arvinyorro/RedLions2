namespace RedLions.Presentation.Web.Areas.Admin.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using RedLions.Presentation.Web.Components;
    using RedLions.Presentation.Web.Models;
    using DTO = RedLions.Application.DTO;

    public class MemberUpdatePoints
    {
        [Required]
        public int MemberUserID { get; set; }

        [Required]
        [Display(Name = "Add/Subtract Points")]
        public int Points { get; set; }
    }
}