namespace RedLions.Infrastructure.Repository
{
    using System.Collections.Generic;
    using RedLions.Business;

    public class InquiryChatRepository : GenericRepository, IInquiryChatRepository
    {
        public InquiryChatRepository(RedLionsContext dbContext)
            : base(dbContext)
        {

        }
    
        public InquiryChatSession GetSessionByID(int id)
        {
            return base.GetById<InquiryChatSession>(id);
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
