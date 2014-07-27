namespace RedLions.Application.DTO
{
    using System;

    public class Payment
    {
        public int ID { get; set; }
        public int PaymentTypeID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PaymentMethod { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string PublicID { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int ReferrerUserID { get; set; }
        public string ReferrerName { get; set; }
        public string ReferrerUnoID { get; set; }
        public bool AdminUnread { get; set; }
        public bool ReferrerUnread { get; set; }
    }
}
