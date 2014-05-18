using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedLions.Business
{
    public interface IInquiryChatRepository
    {
        InquiryChatSession GetSessionByID(int id);

        void CreateSession(InquiryChatSession inquiryChatSession);
        void UpdateSession(InquiryChatSession inquiryChatSession);
    }
}
