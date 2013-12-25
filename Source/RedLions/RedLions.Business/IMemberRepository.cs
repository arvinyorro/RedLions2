namespace RedLions.Business
{
    using System.Collections.Generic;

    public interface IMemberRepository
    {
        void RegisterMember(Member member);
        void Update(Member member);
        IEnumerable<Member> GetAllMembers();
        Member GetMemberByID(int userID);
        Member GetMemberByUsername(string username);
        Member GetMemberByReferralCode(string referralCode);
        Member GetRandomMember();
    }
}
