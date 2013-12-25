namespace RedLions.Application
{
    using System;
    using RedLions.Application.DTO;
    using RedLions.Business;
    
    public class InquiryService
    {
        private IInquiryRepository inquiryRepository;
        private IMemberRepository memberRepository;

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

        public StatusCode SubmitInquiry(DTO.Inquiry inquiryDTO)
        {
            if (inquiryDTO == null)
            {
                throw new ArgumentNullException("inquiryDTO must not be null");
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
