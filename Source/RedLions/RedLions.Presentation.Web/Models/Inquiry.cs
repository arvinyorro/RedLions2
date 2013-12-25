namespace RedLions.Presentation.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DTO = RedLions.Application.DTO;

    public class Inquiry
    {
        // Required by the Mvc default model binder.
        public Inquiry()
        { }

        public Inquiry(DTO.Inquiry inquiryDTO)
        {
            this.ID = inquiryDTO.ID;
            this.FirstName = inquiryDTO.FirstName;
            this.LastName = inquiryDTO.LastName;
            this.CellphoneNumber = inquiryDTO.CellphoneNumber;
            this.Email = inquiryDTO.Email;
            this.ReferrerID = inquiryDTO.ReferrerID;
            this.ReferrerUsername = inquiryDTO.ReferrerUsername;
        }

        public int ID  { get; set; }

        [Required]
        [Display(Name = "First name")]
        [Required(ErrorMessage = "Please enter your first name.")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Please enter your first name.")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        [StringLength(11, MinimumLength=11, ErrorMessage="Invalid Cellphone Number")]
        public string CellphoneNumber { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "You must provide an email.")]
        [MaxLength(200)]
        public string Email { get; set; }

        public int? ReferrerID { get; set; }
        public string ReferrerUsername { get; set; }
    }
}