namespace RedLions.Presentation.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using RedLions.Presentation.Web.Components;
    using RedLions.Presentation.Web.Models;
    using DTO = RedLions.Application.DTO;
    
    public class UpdateMember
    {
        public UpdateMember(
            DTO.Member memberDTO,
            IEnumerable<IDropDown> countryDropDownItems)
        {
            this.ID = memberDTO.ID;
            this.FirstName = memberDTO.FirstName;
            this.LastName = memberDTO.LastName;
            this.Username = memberDTO.Username;
            this.Email = memberDTO.Email;
            this.ReferrerUsername = memberDTO.ReferrerUsername;
            this.CellphoneNumber = memberDTO.CellphoneNumber;
            this.UnoID = memberDTO.UnoID;
            this.Country = new Models.Country(memberDTO.Country);
            this.CountrySelectListItems = countryDropDownItems.ToSelectListItems(memberDTO.Country.ID);
        }

        public UpdateMember()
        {
        }

        public int ID { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The {0} should be between {2} to {1} characters long.")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "You must provide an email.")]
        [MaxLength(200)]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your first name.")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your last name.")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Invalid Cellphone Number")]
        [RegularExpression(@"^09([0-9]){9}$", ErrorMessage = "Invalid Cellphone Format.")]
        public string CellphoneNumber { get; set; }

        [Required]
        [Display(Name = "UNO ID Number")]
        public string UnoID { get; set; }

        [Display(Name = "Referrer")]
        public string ReferrerUsername { get; set; }

        public int? InquiryID { get; set; }

        public Country Country { get; set; }

        public IEnumerable<SelectListItem> CountrySelectListItems { get; set; }
    }
}