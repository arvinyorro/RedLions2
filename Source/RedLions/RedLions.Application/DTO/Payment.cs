namespace RedLions.Application.DTO
{
    using System;

    public class Payment
    {
        public int ID { get; set; }
        public int PaymentTypeID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PaymentMethod { get; set; }
        public string PublicID { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
