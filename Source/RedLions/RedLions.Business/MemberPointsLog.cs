namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MemberPointsLog
    {
        protected MemberPointsLog()
        {
            // Required by EF.
        }

        public MemberPointsLog(User adminUser, Member member, int points)
        {
            this.AdminUser = adminUser;
            this.Member = member;
            this.Points = points;
            this.LoggedDateTime = DateTime.Now;
        }

        public int ID { get; private set; }
        public User AdminUser { get; private set; }
        public Member Member { get; private set; }
        public int Points { get; private set; }
        public DateTime LoggedDateTime { get; private set; }
    }
}
