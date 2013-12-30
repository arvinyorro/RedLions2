namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IMemberRepository
    {
        void RegisterMember(Member member);
        void Update(Member member);

        IEnumerable<Member> GetAllMembers();
        IEnumerable<Member> GetPagedMembers<TKey>(
            int pageIndex,
            int pageSize,
            out int totalCount,
            Expression<Func<Member, TKey>> order,
            Expression<Func<Member, bool>> predicate);

        Member GetMemberByID(int userID);
        Member GetMemberByUsername(string username);
        Member GetMemberByReferralCode(string referralCode);
        Member GetRandomMember();
    }
}
