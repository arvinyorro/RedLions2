namespace RedLions.Presentation.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DTO = RedLions.Application.DTO;

    public class Subscription : IDropDown
    {
        public Subscription()
        {
            // Required by the default MVC model.
        }

        public Subscription(DTO.Subscription subscription)
        {
            this.ID = subscription.ID;
            this.Title = subscription.Title;
        }

        [Required]
        public int ID { get; set; }

        [Display(Name = "Subscription Plan")]
        public string Title { get; set; }
    }
}