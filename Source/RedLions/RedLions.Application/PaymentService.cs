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

        public IEnumerable<DTO.Payment> GetPagedList(
           int pageIndex,
           out int totalCount,
           int pageSize,
           string filterEmail)
        {
            Expression<Func<Business.Payment, bool>> query = PredicateBuilder.True<Business.Payment>();

            if (!string.IsNullOrEmpty(filterEmail))
            {
                query = query.And(x => x.Email.ToUpper().Contains(filterEmail.ToUpper()));
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

        private string GetBody(string fullName, string publicID)
        {
            string body = @"<html>
                <body style=\'font-family: Calibri;\'>
                Dear {full_name},<br />
                <br />
                Thank you for your payment request. We are committed in providing you with the highest level of customer satisfaction possible.<br />
                <br />
                Please submit the reference number from your payment receipt to the following link:<br />
                <br />
                http://unoredlions.com/payment/reference/{public_id}
                <br />
                <br />This is an auto-generated e-mail. Please do not reply to this mail.
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
