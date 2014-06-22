namespace RedLions.Application.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RedLions.Business;

    public class Member : User
    {
        public int MemberID { get; set; }
        public int? InquiryID { get; set; }
        public int ReferralCount { get; set; }
        public string ReferralCode { get; set; }
        public string CellphoneNumber { get; set; }
        public string UnoID { get; set; }
        public string ReferrerUsername { get; set; }
        public DateTime SubscriptionExpirationDateTime { get; set; }
        public Subscription Subscription { get; set; }
        public bool SubscriptionExpired { get; set; }
        public Country Country { get; set; }
    }

    internal static class MemberAssembler
    {
        internal static IEnumerable<DTO.Member> ToDTOList(this IEnumerable<Business.Member> members)
        {
            return members.Select(x => MemberAssembler.ToDTO(x));
        }

        internal static DTO.Member ToDTO(this Business.Member member)
        {
            var memberDTO = new DTO.Member()
            {
                ID = member.ID,
                MemberID = member.MemberID,
                Username = member.Username,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                RegisteredDateTime = member.RegisteredDateTime,
                ReferralCount = member.Referrals.Count,
                ReferralCode = member.ReferralCode,         
                CellphoneNumber = member.CellphoneNumber,
                UnoID = member.UnoID,
                SubscriptionExpirationDateTime = member.SubscriptionExpirationDateTime,
                Country = member.Country.ToDTO(),
                Subscription = member.Subscription.ToDTO(),
                SubscriptionExpired = member.SubscriptionExpired,
            };

            if (member.Referrer != null)
            {
                memberDTO.ReferrerUsername = member.Referrer.Username;
            }

            return memberDTO;
        }
    }
}
