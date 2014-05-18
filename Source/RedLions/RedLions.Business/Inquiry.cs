namespace RedLions.Business
{
    using System;

    public class Inquiry
    {
        protected Inquiry()
        {
            // Required by EF
        }

        public Inquiry(
            string firstName, 
            string lastName, 
            string cellphoneNumber, 
            string email,
            string message,
            Member referrer)
        {
            if (referrer == null)
            {
                throw new ArgumentNullException("referrer must not be null");
            }

            this.FirstName = firstName;
            this.LastName = lastName;
            this.CellphoneNumber = cellphoneNumber;
            this.Email = email;
            this.Referrer = referrer;
            this.Message = message;
            this.InquiredDataTime = DateTime.Now;
            this.Registered = false;   
        }

        public int ID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string CellphoneNumber { get; private set; }
        public string Email { get; private set; }
        public string Message { get; private set; }

        public virtual Member Referrer { get; private set; }
        public DateTime InquiredDataTime { get; private set; }
        public bool Registered { get; private set; }

        internal void Register()
        {
            this.Registered = true;
        }
    }
}
