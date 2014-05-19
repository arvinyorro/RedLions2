using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedLions.Application.DTO
{
    public class InquiryChatSession
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string InquirerName { get; set; }
        public DateTime StartedDateTime { get; set; }
    }
}
