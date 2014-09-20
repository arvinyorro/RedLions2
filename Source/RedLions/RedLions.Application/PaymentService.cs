namespace RedLions.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using RedLions.Application.DTO;
    using RedLions.Business;
    using RedLions.CrossCutting;
    using AutoMapper;

    public class PaymentService
    {
        private IUnitOfWork unitOfWork;
        private IPaymentRepository paymentRepository;
        private IMemberRepository memberRepository;
        private IProductPackageRepository productPackageRepository;
        private IMailClient mailClient;

        public PaymentService(
            IUnitOfWork unitOfWork,
            IPaymentRepository paymentRepository,
            IMemberRepository memberRepository,
            IProductPackageRepository productPackageRepository,
            IMailClient mailClient)
        {
            this.unitOfWork = unitOfWork;
            this.paymentRepository = paymentRepository;
            this.memberRepository = memberRepository;
            this.productPackageRepository = productPackageRepository;
            this.mailClient = mailClient;
        }

        public DTO.Payment GetByID(int id)
        {
            Business.Payment payment = this.paymentRepository.GetByID(id);

            DTO.Payment paymentDto = Mapper.Map<DTO.Payment>(payment);

            return paymentDto;
        }

        public DTO.Payment GetByPublicID(string publicID)
        {
            Business.Payment payment = this.paymentRepository.GetByPublicID(publicID);
            DTO.Payment paymentDto = Mapper.Map<DTO.Payment>(payment);
            return paymentDto;
        }

        public int GetUnreadCount()
        {
            int unreadCount = this.paymentRepository
                .GetUnreadCount(x => x.AdminUnread == true);

            return unreadCount;
        }

        public int GetUnreadCountByMember(int memberUserID)
        {
            int unreadCount = this.paymentRepository
                .GetUnreadCount(x => 
                    x.ReferrerUnread == true && 
                    x.Referrer.ID == memberUserID);
            return unreadCount;
        }

        public IEnumerable<DTO.Payment> GetPagedList(
           int pageIndex,
           out int totalCount,
           int pageSize,
           string filterEmail,
           bool filterUnread)
        {
            Expression<Func<Business.Payment, bool>> query = PredicateBuilder.True<Business.Payment>();

            if (!string.IsNullOrEmpty(filterEmail))
            {
                query = query.And(x => x.Email.ToUpper().Contains(filterEmail.ToUpper()));
            }

            if (filterUnread == true)
            {
                query = query.And(x => x.AdminUnread == true);
            }

            IEnumerable<Business.Payment> payments = this.paymentRepository
                .GetPagedList(
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    totalCount: out totalCount,
                    filter: query);

            return Mapper.Map<IEnumerable<DTO.Payment>>(payments);
        }

        public IEnumerable<DTO.Payment> GetPagedListByMember(
            int memberUserID,
            int pageIndex,
            out int totalCount,
            int pageSize,
            string filterEmail,
            bool filterUnread)
        {
            Expression<Func<Business.Payment, bool>> query = PredicateBuilder.True<Business.Payment>();

            query = query.And(x => x.Referrer.ID == memberUserID);

            if (!string.IsNullOrEmpty(filterEmail))
            {
                query = query.And(x => x.Email.ToUpper().Contains(filterEmail.ToUpper()));
            }

            if (filterUnread == true)
            {
                query = query.And(x => x.ReferrerUnread == true);
            }

            IEnumerable<Business.Payment> payments = this.paymentRepository
                .GetPagedList(
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    totalCount: out totalCount,
                    filter: query);

            return Mapper.Map<IEnumerable<DTO.Payment>>(payments);
        }

        public IEnumerable<DTO.PaymentGift> GetAllGifts()
        {
            var paymentGiftDtoList = new List<DTO.PaymentGift>();

            paymentGiftDtoList.Add(new DTO.PaymentGift() { ID = 1, Title = "GrapeSeed (P600)", Price = 600 });
            paymentGiftDtoList.Add(new DTO.PaymentGift() { ID = 2, Title = "Virgin Coconut Oil (P700)", Price = 700 });
            paymentGiftDtoList.Add(new DTO.PaymentGift() { ID = 3, Title = "Glutathione Capsule (P1300)", Price = 1300 });
            paymentGiftDtoList.Add(new DTO.PaymentGift() { ID = 4, Title = "Fitright (P600)", Price = 600 });
            paymentGiftDtoList.Add(new DTO.PaymentGift() { ID = 5, Title = "Organic Thanakha (P750)", Price = 750 });
            paymentGiftDtoList.Add(new DTO.PaymentGift() { ID = 6, Title = "KryptOrganic (P900)", Price = 900 });
            paymentGiftDtoList.Add(new DTO.PaymentGift() { ID = 7, Title = "Acai Berry (P2500)", Price = 2500 });

            return paymentGiftDtoList;
        }

        public void Create(DTO.Payment paymentDto)
        {
            PaymentType paymentType = paymentDto.PaymentTypeID == 1 ? 
                PaymentType.Cash : PaymentType.PayPal;

            Business.Member referrer = this.memberRepository.GetMemberByID(paymentDto.ReferrerUserID);

            var paymentGifts = new List<Business.PaymentGift>();

            foreach(var paymentGiftDto in paymentDto.GiftCertificates)
            {
                var paymentGift = new Business.PaymentGift(
                    paymentGiftDto.Title, 
                    paymentGiftDto.Quantity,
                    paymentGiftDto.Price);

                paymentGifts.Add(paymentGift);
            }

            Business.ProductPackage productPackage = this.productPackageRepository.GetByID(paymentDto.PackageID);

            if (productPackage == null)
            {
                throw new Exception(string.Format("Product package not found. Package ID: ", paymentDto.PackageID));
            }
            
            var payment = new Business.Payment(
                paymentType: paymentType,
                email: paymentDto.Email,
                firstName: paymentDto.FirstName,
                middleName: paymentDto.MiddleName,
                lastName: paymentDto.LastName,
                age: paymentDto.Age,
                gender: paymentDto.Gender,
                paymentMethod: paymentDto.PaymentMethod,
                mobileNumber: paymentDto.MobileNumber,
                address: paymentDto.Address,
                birthDate: paymentDto.BirthDate,
                referrer: referrer,
                package: productPackage,
                giftCertificates: paymentGifts,
                paymentRepository: this.paymentRepository);

            this.paymentRepository.Create(payment);
            this.unitOfWork.Commit();

            /// Send mail.

            if (paymentType == PaymentType.PayPal) return;

            string fullName = string.Format("{0} {1}",
                    paymentDto.FirstName,
                    paymentDto.LastName);

            string subject = this.GetSubject();
            string body = this.GetBody(fullName, payment.PublicID);

            var recipient = new MailAccount(fullName, paymentDto.Email);
            var sender = new MailAccount("RedLions", "redlions@unoredlions.com");

            Mail mail = new Mail(sender);
            mail.ToRecipients.Add(recipient);
            mail.Subject = subject;
            mail.Body = body;
            this.mailClient.Send(mail);
        }

        public void ConfirmPayment(int id, string referenceNumber)
        {
            Business.Payment payment = this.paymentRepository.GetByID(id);

            payment.Confirm(referenceNumber);

            this.unitOfWork.Commit();
        }

        public void ReadPayment(int paymentID)
        {
            Business.Payment payment = this.paymentRepository.GetByID(paymentID);
            payment.AdminUnread = false;
            this.unitOfWork.Commit();
        }

        public void ReadPaymentByMember(int paymentID)
        {
            Business.Payment payment = this.paymentRepository.GetByID(paymentID);
            payment.ReferrerUnread = false;
            this.unitOfWork.Commit();
        }

        public void ReadAllPayments()
        {
            IEnumerable<Business.Payment> unreadPayments = this.paymentRepository
                .GetUnreadPayments();

            foreach (Business.Payment payment in unreadPayments)
            {
                payment.AdminUnread = false;
            }

            this.unitOfWork.Commit();
        }

        public void ReadAllPaymentsByMember(int memberUserID)
        {
            Business.Member member = this.memberRepository.GetMemberByID(memberUserID);
            IEnumerable<Business.Payment> unreadPayments = this.paymentRepository
                .GetUnreadPaymentsByMember(member);

            foreach(Business.Payment payment in unreadPayments)
            {
                payment.ReferrerUnread = false;
            }

            this.unitOfWork.Commit();
        }

        private string GetBody(string fullName, string publicID)
        {
            string body = @"<html>
                <body style=\'font-family: Calibri;\'>
                Hello {full_name}!<br />
                <br />

                <p>We are pleased to receive your payment request to sign-up as a <b>New Distributor for Unlimited Network of Opportunities Int'l Corp</b>. or 'UNO' thru RedLions group.</p>

                <br />
                <p><b>You opted to make a payment using cash so, please follow instructions below.</b></p>

                <p><b>A.) If you selected to send your payment thru Western Union, Moneygram, Cebuana Lhuiller, ML Kwarta Padala, JRS Pera Padala, LBC Remit Express, or Palawan Express please send it to:</b></p>

                <ul>
                    <li>Name of the Recipient: SAHID M. MAMONDAS</li>
                    <li>Address: c/o UNO INT'L CORP. (HEAD OFFICE), 355 ORTIGAS AVE., BRGY. WACK-WACK, MANDALUYONG CITY 1555, METRO MANILA, PHILIPPINES</li>
                    <li>Contact Number: +639082374424</li>
                </ul>

                <p><b>Note To Option A:</b> <span style=\'color:#C10000;\'>Once you're done, <b style=\'text-decoration: underline;\'>please email your payment receipt to <span style=\'color:#000000;\'>payment.unoredlions@gmail.com</span></b> showing the Name of the Sender and Amount being Sent. Then get back to this email and submit the reference number, as shown on your payment receipt, to the following link --></span> <a href=\'http://unoredlions.com/payment/reference/{public_id}\'>http://unoredlions.com/payment/reference/{public_id}</a></p>

                <br />
                <p><b>B.) If you selected to send your payment thru a Bank Deposit, please take note of the bank account details below:</b></p>

                <ul>
                    <li>Bank Name: BANCO DE ORO</li>
                    <li>Account Number: 004950388692</li>
                    <li>Account Name: WALTER WARREN TRINIDAD</li>
                    <li>Account Type: SAVINGS ACCOUNT</li>
                </ul>


                <p><b>Note To Option B:</b> <span style=\'color:#C10000;\'>After depositing your payment to our bank account, <b style=\'text-decoration: underline;\'>please email your deposit slip to <span style=\'color:#000000;\'>payment.unoredlions@gmail.com</span></b> showing your COMPLETE NAME, UNO ACCOUNT NUMBER, and this Reference Code --></span> <b>{public_id}</b></p>
                <br />
                <p>Thank You! We sincerely appreciate you joining UNO RedLions International.</p>
                <br /><br />
                <p>Wishing You The Best,<br />
                <br />
                Mr. Walter Warren Trinidad, Jr.<br />
                <br />
                President and Grand Upline</p>
                </body>
                <html>";

            body = body.Replace("{full_name}", fullName);
            body = body.Replace("{public_id}", publicID);

            return body;
        }

        private string GetSubject()
        {
            return "RedLions Payment";
        }
    }
}
