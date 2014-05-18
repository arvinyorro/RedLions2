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
        public string Name { get; set; }
    }
}