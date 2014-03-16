﻿namespace RedLions.Presentation.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;
    using System.Web.Mvc;
    using DTO = RedLions.Application.DTO;

    public class Member : User
    {
        public Member(DTO.Member memberDTO)
        {   
            this.ID = memberDTO.ID;
            this.FirstName = memberDTO.FirstName;
            this.LastName = memberDTO.LastName;
            this.FullName = string.Format("{0} {1}",
                memberDTO.FirstName,
                memberDTO.LastName);
            this.Username = memberDTO.Username;
            this.Email = memberDTO.Email;
            this.RegisteredDateTime = memberDTO.RegisteredDateTime;
            this.ReferralCount = memberDTO.ReferralCount;
            this.ReferralCode = memberDTO.ReferralCode;
            this.ReferrerUsername = memberDTO.ReferrerUsername ?? "No referral";
            var request = HttpContext.Current.Request;
            this.ReferralLink = string.Format("{0}/{1}",
                request.Url.GetLeftPart(UriPartial.Authority), memberDTO.Username);
            this.CellphoneNumber = memberDTO.CellphoneNumber;
            this.UnoID = memberDTO.UnoID;
            this.Country = new Models.Country(memberDTO.Country);
        }

        public Member()
        {
            // Required by MVC default data binding.
        }

        public int ID { get; set; }

        public int? InquiryID { get; set; }

        [Display(Name="Name")]
        public string FullName { get; set; }

        [Display(Name = "Referral Code")]
        public string ReferralCode { get; set; }

        [Display(Name = "Referral Link")]
        public string ReferralLink { get; set; }

        [Display(Name = "Total Referrals")]
        public int ReferralCount { get; set; }

        [Display(Name = "Referrer")]
        public string ReferrerUsername { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Invalid Cellphone Length.")]
        [RegularExpression(@"^09([0-9]){9}$", ErrorMessage = "Invalid Cellphone Format.")]
        public string CellphoneNumber { get; set; }

        [Required]
        [Display(Name = "UNO ID Number")]
        public string UnoID { get; set; }

        public Country Country { get; set; }
    }
}