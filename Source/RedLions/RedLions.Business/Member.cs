namespace RedLions.Business
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
            string personalReferralCode)
            : base(username, firstName, lastName, email, password)
        {
            this.Role = Role.Member;
            this.ReferralCode = personalReferralCode;
        }

        public Member(
            Inquiry inquiry,
            string username,
            string password,
            string personalReferralCode)
            : base(username, inquiry.FirstName, inquiry.LastName, inquiry.Email, password)
        {
            this.Role = Role.Member;
            this.ReferralCode = personalReferralCode;
            inquiry.Register();
        }

        public Member()
        {
        }      

        public int MemberID { get; private set; }
        public string ReferralCode { get; private set; }

        // Must be optional because the very first member has no referrer.
        public virtual Member Referrer { get; set; }
        public virtual ICollection<Member> Referrals { get; private set; }
    }
}
