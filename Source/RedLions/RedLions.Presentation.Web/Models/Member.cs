namespace RedLions.Presentation.Web.Models
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
            this.DeliveryAddress = memberDTO.DeliveryAddress;
            this.HomeAddress = memberDTO.HomeAddress;
            this.Nationality = memberDTO.Nationality;
            this.UnoID = memberDTO.UnoID;
            this.SubscriptionExpirationDateTime = memberDTO.SubscriptionExpirationDateTime;
            this.Subscription = new Models.Subscription(memberDTO.Subscription);
            this.SubscriptionStatus = memberDTO.SubscriptionExpired == true ? "Expired" : "Active";
            this.Country = new Models.Country(memberDTO.Country);
            this.Points = memberDTO.Points;
            this.Deactivated = memberDTO.Deactivated;
        }

        public Member()
        {
            // Required by MVC default data binding.
        }

        public int ID { get; set; }

        public int? InquiryID { get; set; }

        [Display(Name="Complete Name")]
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
        [StringLength(20, MinimumLength = 11, ErrorMessage = "Invalid Cellphone Length.")]
        public string CellphoneNumber { get; set; }

        [Required]
        [Display(Name = "Home Address")]
        public string HomeAddress { get; set; }

        [Required]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; }

        [Required]
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Required]
        [Display(Name = "Uno Id")]
        public string UnoID { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime SubscriptionExpirationDateTime { get; set; }

        [Display(Name = "Plan")]
        public Subscription Subscription { get; set; }

        [Display(Name = "Status")]
        public string SubscriptionStatus { get; set; }

        public Country Country { get; set; }

        [Display(Name = "Points")]
        public int Points { get; set; }

        [Display(Name = "Deactivated")]
        public bool Deactivated { get; set; }
    }
}