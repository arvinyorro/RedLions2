namespace RedLions.Presentation.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;
    using System.Web.Mvc;
    using DTO = RedLions.Application.DTO;

    public class InquiryChatRegister
    {
        [Display(Name = "Name")]
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "No special characters allowed.")]
        public string Name { get; set; }
    }
}