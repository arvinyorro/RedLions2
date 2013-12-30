using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedLions.Presentation.Web.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DTO = RedLions.Application.DTO;

    public class EditMember
    {
        public EditMember(DTO.Member memberDTO)
        {
            this.ID = memberDTO.ID;
            this.FirstName = memberDTO.FirstName;
            this.LastName = memberDTO.LastName;
            this.Username = memberDTO.Username;
            this.Email = memberDTO.Email;
            this.ReferrerUsername = memberDTO.ReferrerUsername;
            this.CellphoneNumber = memberDTO.CellphoneNumber;
        }

        public EditMember()
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

        [Display(Name = "Referrer")]
        public string ReferrerUsername { get; set; }
    }
}