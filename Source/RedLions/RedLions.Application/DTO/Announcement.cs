namespace RedLions.Application.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Announcement
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public User Poster { get; set; } 
        public DateTime PostedDateTime { get; set; }
    }
}
