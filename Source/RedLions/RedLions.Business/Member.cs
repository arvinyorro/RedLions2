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

        public Member()
        {
        }
               

        public int MemberID { get; private set; }
        public string ReferralCode { get; private set; }
        public virtual Member Referrer { get; set; }
        public virtual ICollection<Member> Referrals { get; private set; }
    }
}
