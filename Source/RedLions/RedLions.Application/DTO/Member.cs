namespace RedLions.Application.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RedLions.Business;

    public class Member : User
    {
        public int? InquiryID { get; set; }
        public int ReferralCount { get; set; }
        public string ReferralCode { get; set; }
        public string CellphoneNumber { get; set; }
        public string ReferrerUsername { get; set; }
    }

    internal static class MemberAssembler
    {
        internal static IEnumerable<DTO.Member> ToDTOList(IEnumerable<Business.Member> members)
        {
            return members.Select(x => MemberAssembler.ToDTO(x));
        }

        internal static DTO.Member ToDTO(Business.Member member)
        {
            var memberDTO = new DTO.Member()
            {
                ID = member.ID,
                Username = member.Username,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                RegisteredDateTime = member.RegisteredDateTime,
                ReferralCount = member.Referrals.Count,
                ReferralCode = member.ReferralCode,         
                CellphoneNumber = member.CellphoneNumber,
            };

            if (member.Referrer != null)
            {
                memberDTO.ReferrerUsername = member.Referrer.Username;
            }

            return memberDTO;
        }
    }
}
