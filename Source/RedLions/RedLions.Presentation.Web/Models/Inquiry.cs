﻿namespace RedLions.Presentation.Web.Models
{
    using System;
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

            this.Name = string.Format("{0} {1}", inquiryDTO.FirstName, inquiryDTO.LastName);
            this.InquiredDateTime = inquiryDTO.InquiredDateTime;
        }

        public int ID  { get; set; }

        public string Name { get; set; }

        [Display(Name="Date Inquired")]
        public DateTime InquiredDateTime { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Please enter your first name.")]
        [MaxLength(100)]
        public string FirstName { get; set; }

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