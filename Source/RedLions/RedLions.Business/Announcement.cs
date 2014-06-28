namespace RedLions.Business
{
    using System;

    public class Announcement
    {
        protected Announcement()
        {
            // Required by EF.
        }

        public Announcement(
            string title, 
            string message, 
            User poster)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }

            if (poster == null)
            {
                throw new ArgumentNullException("poster");
            }

            this.UserPoster = poster;
            this.Title = title;
            this.Message = message;
            this.PostedDateTime = DateTime.Now;
        }

        public int ID { get; private set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime PostedDateTime { get; private set; }

        // Navigational Models
        public virtual User UserPoster { get; private set; }
    }
}
