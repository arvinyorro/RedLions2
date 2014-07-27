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
        private IMailClient mailClient;

        public PaymentService(
            IUnitOfWork unitOfWork,
            IPaymentRepository paymentRepository,
            IMemberRepository memberRepository,
            IMailClient mailClient)
        {
            this.unitOfWork = unitOfWork;
            this.paymentRepository = paymentRepository;
            this.memberRepository = memberRepository;
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

        public void Create(DTO.Payment paymentDto)
        {
            PaymentType paymentType = paymentDto.PaymentTypeID == 1 ? 
                PaymentType.Cash : PaymentType.PayPal;

            Business.Member referrer = this.memberRepository.GetMemberByID(paymentDto.ReferrerUserID);

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
                We received your payment request to sign-up for Local/International Account with Unlimited Network of Opportunities Int'l Corp. or 'UNO' thru RedLions group taking <PRODUCT PACKAGE> as your chosen Distributorship Product Package.<br />
                <br />
                <p>A.) If you selected to send your payment thru Western Union, Moneygram, Cebuana Lhuiller, ML Kwarta Padala, JRS Pera Padala, LBC Remit Express, or Palawan Express please use the details below:<p>
                <ul>
                    <li>Name of the Recipient: WALTER WARREN TRINIDAD JR.</li>
                    <li>Address: c/o UNO INT'L CORP., #355 ORTIGAS AVE., BRGY. WACK-WACK, MANDALUYONG CITY 1555, METRO MANILA, PHILIPPINES</li>
                    <li>Contact Number: +639082374424</li>
                </ul>
                <p>Note: Once you're done please submit the reference number, as shown on your payment receipt, to the following link: </p>
                http://unoredlions.com/payment/reference/{public_id}
                <br />
                <p>B.) If you selected to send your payment thru a Bank Deposit, please take note of the bank account details below:</p>

                <ul>
                    <li>Bank Name: Banco de Oro</li>
                    <li>Account Number: 004950388692</li>
                    <li>Account Name: Walter Warren Trinidad</li>
                </ul>

                <p>Note: After depositing your payment please email us your deposit slip with your COMPLETE NAME, UNO ACCOUNT NUMBER, and this Reference Code: <b>{public_id}</b>.</p>

                <br />Thank You! We sincerely appreciate you joining RedLions International.
                </body>
                <html> ";

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
