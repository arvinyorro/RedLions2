namespace RedLions.Business
{
    using System;
    using System.Linq;
    using RedLions.CrossCutting;

    public class Payment
    {
        protected Payment()
        {
            // Required by EF.
        }

        public Payment(
            PaymentType paymentType,
            string email,
            string firstName,
            string lastName,
            int age,
            string gender,
            string paymentMethod,
            string mobileNumber,
            string address,
            DateTime birthDate,
            Member referrer,
            IPaymentRepository paymentRepository,
            string middleName = null)
        {
            this.Type = paymentType;
            this.FirstName = firstName;
            this.MiddleName = middleName;
            this.LastName = lastName;
            this.Email = email;
            this.Age = age;
            this.Gender = gender;
            this.PaymentMethod = paymentMethod;
            this.MobileNumber = mobileNumber;
            this.Address = address;
            this.BirthDate = birthDate;
            this.Referrer = referrer;
            this.CreatedDateTime = SystemTime.Now;
            this.PublicID = this.GeneratePublicID(paymentRepository);
        }

        public int ID { get; private set; }
        public int PaymentTypeID { get; private set; }
        public PaymentType Type
        {
            get
            {
                switch (this.PaymentTypeID)
                {
                    case 1:
                        return PaymentType.Cash;
                    case 2:
                        return PaymentType.PayPal;
                    default:
                        throw new Exception("Unknown payment type ID");
                }
            }

            private set
            {
                switch (value)
                {
                    case PaymentType.Cash:
                        this.PaymentTypeID = 1;
                        break;
                    case PaymentType.PayPal:
                        this.PaymentTypeID = 2;
                        break;
                    default:
                        throw new Exception("Unknown payment type");
                }
            }
        }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string MiddleName { get; private set; }
        public string LastName { get; private set; }
        public int Age { get; private set; }
        public string Gender { get; private set; }
        public string PaymentMethod { get; private set; }
        public string MobileNumber { get; private set; }
        public string Address { get; private set; }
        public string PublicID { get; private set; }
        public string ReferenceNumber { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Member Referrer { get; private set; }

        public void Confirm(string referenceNumber)
        {
            this.ReferenceNumber = referenceNumber;
        }

        private string GeneratePublicID(IPaymentRepository paymentRepository)
        {
            int publicIDLength = 50;
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string publicID = string.Empty;

            do
            {
                publicID = new string(
                    Enumerable.Repeat(characters, publicIDLength)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());
            }
            while (paymentRepository.GetByPublicID(publicID) != null);

            return publicID;
        }
    }
}
