namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

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
