﻿namespace RedLions.Presentation.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DTO = RedLions.Application.DTO;

    public class Payment
    {
        public int ID { get; set; }

        public bool Local { get; set; }

        [Required]
        public int PaymentTypeID { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(100)]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [StringLength(1)]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        [StringLength(20)]
        public string MobileNumber { get; set; }

        [Required]
        [Display(Name = "Complete Address")]
        [StringLength(300)]
        public string Address { get; set; }

        public int PayPalID { get; set; }

        [Display(Name = "Tracking Number")]
        public string PublicID { get; set; }

        [Display(Name = "Reference Code")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Request Made (Date & Time)")]
        public DateTime CreatedDateTime { get; set; }

        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public int ReferrerUserID { get; set; }

        [Display(Name = "Referrer Name")]
        public string ReferrerName { get; set; }

        [Display(Name = "Referrer UNO ID")]
        public string ReferrerUnoID { get; set; }

        public bool AdminUnread { get; set; }

        public bool ReferrerUnread { get; set; }

        [Required]
        public int PackageID { get; set; }

        [Display(Name = "Selected Product Package")]
        public string PackageTitle { get; set; }

        public IEnumerable<Models.PaymentGift> GiftCertificates { get; set; }

        public void AddReferrer(DTO.Member referrer)
        {
            this.ReferrerName = string.Format("{0} {1}", referrer.FirstName, referrer.LastName);
            this.ReferrerUnoID = referrer.UnoID;
            this.ReferrerUserID = referrer.ID;
        }
    }
}