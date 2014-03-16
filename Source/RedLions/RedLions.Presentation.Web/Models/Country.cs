namespace RedLions.Presentation.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DTO = RedLions.Application.DTO;

    public class Country :  IDropDown
    {
        public Country()
        {
            // Required by the default MVC model.
        }

        public Country(DTO.Country countryDTO)
        {
            this.ID = countryDTO.ID;
            this.Title = countryDTO.Title;
        }

        [Required]
        public int ID { get; set; }

        [Display(Name="Country")]
        public string Title { get; set; }
    }
}