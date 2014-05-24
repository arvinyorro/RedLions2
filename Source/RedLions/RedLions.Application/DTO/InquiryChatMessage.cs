using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedLions.Application.DTO
{
    public class InquiryChatMessage
    {
        public int ID { get; set; }
        public int InquiryChatSessionID { get; set; }
        public string SenderUsername { get; set; }
        public string Message { get; set; }
        public DateTime SentDateTime { get; set; }
    }
}
