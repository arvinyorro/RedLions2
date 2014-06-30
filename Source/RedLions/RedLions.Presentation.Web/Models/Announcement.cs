namespace RedLions.Presentation.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DTO = RedLions.Application.DTO;

    public class Announcement
    {
        public Announcement()
        {
            // Required by the default MVC data binding.
        }

        public virtual int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(600)]
        public string Message { get; set; }

        [Display(Name="Date Posted")]
        public DateTime PostedDateTime { get; set; }
    }
}