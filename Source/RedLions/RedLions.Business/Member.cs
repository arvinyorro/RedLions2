﻿namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Member : User
    {
        public Member(
            string username,
            string firstName,
            string lastName,
            string email,
            string password,
            string personalReferralCode,
            string cellphoneNumber,
            Inquiry inquiry = null)
            : base(username, firstName, lastName, email, password)
        {
            if (inquiry != null)
            {
                this.Inquiry = inquiry;
                inquiry.Register();
            }

            this.Role = Role.Member;
            this.ReferralCode = personalReferralCode;
            this.CellphoneNumber = cellphoneNumber;
        }

        public Member()
        {
        }      

        public IEnumerable<Member> GetPagedReferrals(int pageIndex, int pageSize)
        {
            pageIndex = (pageIndex <= 0 ? 1 : pageIndex) - 1;
            return this.Referrals.OrderBy(x => x.ID).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public int MemberID { get; private set; }
        public string ReferralCode { get; private set; }
        public string CellphoneNumber { get; set; }

        // Must be optional because the very first member has no referrer.
        public virtual Member Referrer { get; set; }

        public virtual ICollection<Member> Referrals { get; private set; }
        public virtual Inquiry Inquiry { get; private set; }
    }
}
