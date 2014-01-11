namespace RedLions.Application
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;  
    using RedLions.Application.DTO;
    using RedLions.CrossCutting;
    using RedLions.Business;
    
    public class InquiryService
    {
        private IInquiryRepository inquiryRepository;
        private IMemberRepository memberRepository;
        private int pageSize = 10;

        public InquiryService(
            IInquiryRepository inquiryRepository,
            IMemberRepository memberRepository)
        {
            if (inquiryRepository == null)
            {
                throw new ArgumentNullException("The parameter 'inquiryRepository' must not be null");
            }

            if (memberRepository == null)
            {
                throw new ArgumentNullException("The parameter 'memberRepository' must not be null");
            }

            this.inquiryRepository = inquiryRepository;
            this.memberRepository = memberRepository;
        }

        public int PageSize
        {
            get
            {
                return pageSize;
            }
        }

        public StatusCode SubmitInquiry(DTO.Inquiry inquiryDTO)
        {
            if (inquiryDTO == null)
            {
                throw new ArgumentNullException("inquiryDTO must not be null.");
            }

            Business.Member referrer = this.GenerateReferrer(inquiryDTO.ReferrerID);

            var inquiry = new Business.Inquiry(
                firstName: inquiryDTO.FirstName,
                lastName: inquiryDTO.LastName,
                cellphoneNumber: inquiryDTO.CellphoneNumber,
                email: inquiryDTO.Email,
                message: inquiryDTO.Message,
                referrer: referrer);

            this.inquiryRepository.Inquire(inquiry);

            return StatusCode.Success;
        }

        public ICollection<DTO.Inquiry> GetPagedInquiries(
            int pageIndex,
            out int totalCount,
            string filterEmail)
        {
            Expression<Func<Business.Inquiry, bool>> query = PredicateBuilder.True<Business.Inquiry>();

            if (!string.IsNullOrEmpty(filterEmail))
            {
                query = query.And(x => x.Email.ToUpper().Contains(filterEmail.ToUpper()));
            }

            ICollection<DTO.Inquiry> inquiryDTOs = this
                .GetPagedInquiries(
                    pageIndex,
                    out totalCount,
                    query)
                .ToDTOList()
                .ToList();

            return inquiryDTOs;
        }

        public ICollection<DTO.Inquiry> GetPagedInquiriesByMember(
            int memberUserID,
            int pageIndex,
            out int totalCount,
            string filterEmail)
        {
            Expression<Func<Business.Inquiry, bool>> query = PredicateBuilder.True<Business.Inquiry>();

            if (!string.IsNullOrEmpty(filterEmail))
            {
                query = query.And(x => x.Email.ToUpper().Contains(filterEmail.ToUpper()));
            }

            Business.Member referrer = this.memberRepository.GetMemberByID(memberUserID);
            if (referrer == null)
            {
                throw new Exception("Referrer of inquiry not found.");
            }

            query = query.And(x => x.Referrer.ID == referrer.ID);

            ICollection<DTO.Inquiry> inquiryDTOs = this
                .GetPagedInquiries(
                    pageIndex,
                    out totalCount,
                    query)
                .ToDTOList()
                .ToList();

            return inquiryDTOs;
        }

        public DTO.Inquiry GetById(int inquiryId)
        {
            Business.Inquiry inquiry = this.inquiryRepository.GetById(inquiryId);
            if (inquiry == null) return null;

            return inquiry.ToDTO();
        }

        private Business.Member GenerateReferrer(int? referrerID)
        {
            if (!referrerID.HasValue)
            {
                return this.memberRepository.GetRandomMember();
            }

            Business.Member referrer = this.memberRepository.GetMemberByID(referrerID.Value);
            if (referrer == null)
            {
                throw new Exception("Referrer specified was not found");
            }
            return referrer;
        }

        private IEnumerable<Business.Inquiry> GetPagedInquiries(
            int pageIndex,
            out int totalCount,
            Expression<Func<Business.Inquiry, bool>> query)
        {
            query = query.And(x => x.Registered == false);

            return this.inquiryRepository
                .GetPagedList(
                    pageIndex,
                    this.pageSize,
                    out totalCount,
                    x => x.InquiredDataTime,
                    query);
        }
    }
}
