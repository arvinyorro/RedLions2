namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RedLions.CrossCutting;

    public class Member : User
    {
        public Member(
            string username,
            string unoID,
            string firstName,
            string lastName,
            string email,
            string personalReferralCode,
            string cellphoneNumber,
            Subscription subscription,
            Country country,
            Inquiry inquiry = null)
            : base(username, firstName, lastName, email)
        {
            if (inquiry != null)
            {
                this.Inquiry = inquiry;
                inquiry.Register();
            }

            this.Role = Role.Member;
            this.ReferralCode = personalReferralCode;
            this.CellphoneNumber = cellphoneNumber;
            this.Country = country;
            this.UnoID = unoID;
            this.Subscription = subscription;
            this.SubscriptionExpirationDateTime = SystemTime.Now.AddMonths(subscription.Months);
            this.Deactivated = false;
        }

        protected Member()
        {
            // Required by the EF.
        }      
        
        public int MemberID { get; private set; }

        public string ReferralCode { get; private set; }

        public string CellphoneNumber { get; set; }

        public string UnoID { get; set; }

        public bool SubscriptionExpired
        {
            get
            {
                if (this.SubscriptionExpirationDateTime < SystemTime.Now)
                {
                    return true;
                }

                return false;  
            }
        }

        public int Points { get; private set; }

        public bool Deactivated { get; private set; }

        public DateTime? DeactivatedDateTime { get; set; }

        public DateTime SubscriptionExpirationDateTime { get; set; }

        /// <remarks>
        /// Must be optional because the very first member has no referrer.
        /// </remarks>
        public virtual Member Referrer { get; set; }

        public virtual Inquiry Inquiry { get; private set; }

        public virtual Country Country { get; set; }

        public virtual Subscription Subscription { get; set; }

        public virtual ICollection<Member> Referrals { get; private set; }

        public virtual ICollection<MemberPointsLog> MemberPointsLogs { get; private set; }

        public IEnumerable<Member> GetPagedReferrals(int pageIndex, int pageSize)
        {
            // Why did I did this again?
            pageIndex = (pageIndex <= 0 ? 1 : pageIndex) - 1;
            return this.Referrals.OrderBy(x => x.ID).Skip(pageIndex * pageSize).Take(pageSize);
        }

        public void ExtendSubscription(Subscription subscription)
        {
            this.SubscriptionExpirationDateTime = this.SubscriptionExpirationDateTime.AddMonths(subscription.Months);
            this.Subscription = subscription;
        }

        public void AddPoints(User adminUser, int points)
        {
            var memberPointsLog = new MemberPointsLog(adminUser, this, points);
            this.MemberPointsLogs.Add(memberPointsLog);

            // Update total points.
            this.Points = this.MemberPointsLogs.Select(x => x.Points).Sum();
        }

        public void Activate()
        {
            TimeSpan remainingSubscription = this.SubscriptionExpirationDateTime.Subtract(this.DeactivatedDateTime.Value);
            this.SubscriptionExpirationDateTime = SystemTime.Now.Add(remainingSubscription);
            this.Deactivated = false;
        }

        public void Deactivate()
        {
            this.DeactivatedDateTime = SystemTime.Now;
            this.Deactivated = true;
        }
    }
}
