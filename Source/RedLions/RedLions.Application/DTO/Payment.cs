namespace RedLions.Application.DTO
{
    using System;

    public class Payment
    {
        public int ID { get; private set; }
        public int PaymentTypeID { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int Age { get; private set; }
        public string Gender { get; private set; }
        public string PaymentMethod { get; private set; }
        public string PublicID { get; private set; }
        public string ReferenceNumber { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
    }
}
