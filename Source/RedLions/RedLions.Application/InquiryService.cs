namespace RedLions.Application
{
    using System;
    using System.Linq;
    using System.Collections.Generic;  
    using RedLions.Application.DTO;
    using RedLions.Business;
    
    public class InquiryService
    {
        private IInquiryRepository inquiryRepository;
        private IMemberRepository memberRepository;
        private const int pageSize = 10;

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
                referrer: referrer
                );

            this.inquiryRepository.Inquire(inquiry);

            return StatusCode.Success;
        }

        public ICollection<DTO.Inquiry> GetPagedInquiries(
            int pageIndex,
            out int totalCount)
        {
            totalCount = 0;

            IEnumerable<Business.Inquiry> inquiries = this.inquiryRepository
                .GetPagedList(
                    pageIndex,
                    pageSize,
                    out totalCount,
                    x => x.InquiredDataTime,
                    x => x.Registered == false);

            return InquiryAssembler.ToDTOList(inquiries).ToList();
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
    }
}
