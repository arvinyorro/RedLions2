namespace RedLions.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using RedLions.Business;

    /// <summary>
    /// This class implements the <see cref="RedLions.Business.IMemberRepository"/> interface.
    /// </summary>
    public class MemberRepository : GenericRepository, IMemberRepository
    {
        private RedLionsContext context;

        public MemberRepository(RedLionsContext context)
            : base(context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("The parameter 'context' must not be null");
            }

            this.context = context;
        }

        public void RegisterMember(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("The parameter 'member' must not be null");
            }

            base.Create<Member>(member);
        }

        public void Update(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("The parameter 'member' must not be null");
            }

            base.Update<Member>(member);
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return base.GetAll<Member>();
        }

        public IEnumerable<Member> GetPagedMembers<TKey>(
            int pageIndex,
            int pageSize,
            out int totalCount,
            Expression<Func<Member, TKey>> order,
            Expression<Func<Member, bool>> predicate)
        {
            return base.GetPagedList<Member, TKey>(pageIndex, pageSize, out totalCount, order, predicate);
        }

        public Member GetMemberByID(int userID)
        {
            return base.GetById<Member>(userID);
        }

        public Member GetMemberByUsername(string username)
        {
            return base.GetSingle<Member>(x => x.Username == username);
        }

        public Member GetMemberByReferralCode(string referralCode)
        {
            return base.GetSingle<Member>(x => x.ReferralCode == referralCode);
        }

        public Member GetRandomMember()
        {
            int maxUserCount = this.context.Users.OfType<Member>().Count();
            int randomCount = new Random().Next(maxUserCount);

            return this.context.Users.OfType<Member>()
                .OrderBy(x => x.ID).Skip(randomCount).First();
        }

    }
}
