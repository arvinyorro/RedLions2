﻿namespace RedLions.Application
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
        private IMailClient mailClient;

        public PaymentService(
            IUnitOfWork unitOfWork,
            IPaymentRepository paymentRepository,
            IMailClient mailClient)
        {
            this.unitOfWork = unitOfWork;
            this.paymentRepository = paymentRepository;
            this.mailClient = mailClient;
        }

        public DTO.Payment GetByID(int id)
        {
            Business.Payment payment = this.paymentRepository.GetByID(id);

            DTO.Payment paymentDto = Mapper.Map<DTO.Payment>(payment);

            return paymentDto;
        }

        public IEnumerable<DTO.Payment> GetPagedList(
           int pageIndex,
           out int totalCount,
           int pageSize)
        {
            IEnumerable<Business.Payment> payments = this.paymentRepository
                .GetPagedList(
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    totalCount: out totalCount);

            return Mapper.Map<IEnumerable<DTO.Payment>>(payments);
        }

        public void Create(DTO.Payment paymentDto)
        {
            PaymentType paymentType = paymentDto.PaymentTypeID == 1 ? 
                PaymentType.Cash : PaymentType.PayPal;

            var payment = new Business.Payment(
                paymentType: paymentType,
                email: paymentDto.Email,
                firstName: paymentDto.FirstName,
                lastName: paymentDto.LastName,
                age: paymentDto.Age,
                gender: paymentDto.Gender,
                paymentMethod: paymentDto.PaymentMethod,
                paymentRepository: this.paymentRepository);

            this.paymentRepository.Create(payment);
            this.unitOfWork.Commit();

            /// Send mail.

            string fullName = string.Format("{0} {1}",
                    paymentDto.FirstName,
                    paymentDto.LastName);

            string subject = this.GetSubject();
            string body = this.GetBody(fullName);

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

        private string GetBody(string fullName)
        {
            string body = @"<html>
                <body style=\'font-family: Calibri;\'>
                Dear {full_name},<br />
                <br />
                Thank you for your payment. We are committed in providing you with the highest level of customer satisfaction possible.<br />
                <br />
                Please confirm your payment by submitting the reference number in your receipt to the following link:<br />
                <br />
                http://redlions.com/payment/confirm/1231512321
                <br />
                <br />This is an auto-generated e-mail. Please do not reply to this mail.
                </body>
                <html> ";

            body.Replace("{full_name}", fullName);

            return body;
        }

        private string GetSubject()
        {
            return "RedLions Payment";
        }
    }
}
