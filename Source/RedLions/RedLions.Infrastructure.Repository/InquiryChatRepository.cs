namespace RedLions.Infrastructure.Repository
{
    using System.Collections.Generic;
    using RedLions.Business;
    using RedLions.CrossCutting;

    public class InquiryChatRepository : GenericRepository, IInquiryChatRepository
    {
        public InquiryChatRepository(IDbContext dbContext)
            : base(dbContext)
        {

        }
    
        public InquiryChatSession GetSessionByID(int id)
        {
            return base.GetById<InquiryChatSession>(id);
        }

        public IEnumerable<InquiryChatSession> GetSessionsByMember(Member member)
        {
            return base.GetAll<InquiryChatSession>(x => x.Member.ID == member.ID);
        }

        public void CreateSession(InquiryChatSession inquiryChatSession)
        {
            base.Create<InquiryChatSession>(inquiryChatSession);
        }

        public void UpdateSession(InquiryChatSession inquiryChatSession)
        {
            base.Update<InquiryChatSession>(inquiryChatSession);
        }
    }
}
