namespace RedLions.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using RedLions.CrossCutting;
    using RedLions.Application.DTO;
    using RedLions.Business;

    public enum MemberCreateStatus
    {
        Success = 1,
    }

    public class MemberService
    {
        private int pageSize = 10;
        private const int ReferralCodeLength = 20;
        private IRepository genericRepository;
        private IMemberRepository memberRepository;
        private IUserRepository userRepository;

        public MemberService(
            IRepository genericRepository,
            IUserRepository userRepository,
            IMemberRepository memberRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException("The parameter 'userRepository' must not be null");
            }

            if (memberRepository == null)
            {
                throw new ArgumentNullException("The parameter 'memberRepository' must not be null");
            }

            if (genericRepository == null)
            {
                throw new ArgumentNullException("The parameter 'memberRepository' must not be null");
            }

            this.genericRepository = genericRepository;
            this.userRepository = userRepository;
            this.memberRepository = memberRepository;
        }

        public int PageSize
        {
            get
            {
                return pageSize;
            }
        }

        public IEnumerable<DTO.Member> GetAllMembers()
        {
            IEnumerable<Business.Member> members = this.memberRepository.GetAllMembers();
            return MemberAssembler.ToDTOList(members);
        }

        public IEnumerable<DTO.Member> GetPagedMembers(
            int pageIndex,
            out int totalCount,
            string username)
        {
            Expression<Func<Business.Member, bool>> query = PredicateBuilder.True<Business.Member>();

            if (!string.IsNullOrEmpty(username))
            {
                query = query.And(x => x.Username.ToUpper().Contains(username.ToUpper()));
            }

            IEnumerable<Business.Member> members = this.memberRepository
                .GetPagedMembers(
                    pageIndex, 
                    PageSize, 
                    out totalCount,
                    x => x.ID,
                    query);

            return members.ToDTOList();
        }

        public DTO.Member GetMemberByID(int userID)
        {
            Business.Member member = this.memberRepository.GetMemberByID(userID);
            return MemberAssembler.ToDTO(member);
        }

        public DTO.Member GetMemberByUsername(string username)
        {
            Business.Member member = this.memberRepository.GetMemberByUsername(username);

            if (member == null)
            {
                return null;
            }

            return MemberAssembler.ToDTO(member);
        }

        public DTO.Member GetMemberByReferralCode(string referralCode)
        {
            Business.Member member = this.memberRepository.GetMemberByReferralCode(referralCode);

            if (member == null) return null;

            return MemberAssembler.ToDTO(member);
        }

        public IEnumerable<DTO.Member> GetReferrals(
            int pageIndex,
            out int totalCount,
            int referrerUserID)
        {
            Business.Member member = this.memberRepository.GetMemberByID(referrerUserID);

            totalCount = member.Referrals.Count;

            IEnumerable<Business.Member> referrals = member.GetPagedReferrals(pageIndex, this.pageSize);

            return referrals.ToDTOList();
        }

        public StatusCode Register(DTO.Member memberDTO)
        {
            var statusCode = StatusCode.Success;

            // Add validations here
            this.Validate(memberDTO);

            if (memberDTO.Password == null)
            {
                memberDTO.Password = "redlions";
            }

            Business.Inquiry inquiry = null;
            if (memberDTO.InquiryID.HasValue)
            {
                inquiry = this.genericRepository.GetById<Business.Inquiry>(memberDTO.InquiryID.Value);
            }

            var member = new Business.Member(
                inquiry: inquiry,
                username: memberDTO.Username,
                firstName: memberDTO.FirstName,
                lastName: memberDTO.LastName,
                email: memberDTO.Email,
                password: Password.Encrypt(memberDTO.Password),
                personalReferralCode: this.GenerateReferralCode(),
                cellphoneNumber: memberDTO.CellphoneNumber);

            member.Referrer = this.GenerateReferrer(out statusCode, memberDTO.ReferrerUsername);

            if (statusCode == StatusCode.Success)
            {
                this.memberRepository.RegisterMember(member);
            }

            return statusCode;
        }

        public StatusCode Update(DTO.Member memberDTO)
        {
            var statusCode = StatusCode.Success;

            // Add validations here
            statusCode = this.Validate(memberDTO);
            if (statusCode != StatusCode.Success)
            {
                return statusCode;
            }

            Business.Member member = this.memberRepository.GetMemberByID(memberDTO.ID);

            member.Username = memberDTO.Username;
            member.FirstName = memberDTO.FirstName;
            member.LastName = memberDTO.LastName;
            member.Email = memberDTO.Email;
            member.CellphoneNumber = memberDTO.CellphoneNumber;

            if (member.Referrer != null &&
                member.Referrer.Username != memberDTO.ReferrerUsername)
            {
                member.Referrer = this.GenerateReferrer(out statusCode, memberDTO.ReferrerUsername);
            }

            if (statusCode == StatusCode.Success)
            {
                this.memberRepository.Update(member);
            }

            return statusCode;
        }

        private StatusCode Validate(DTO.Member memberDTO)
        {
            if (memberDTO == null)
            {
                throw new ArgumentNullException("The parameter 'memberRepository' must not be null");
            }

            if (memberDTO.ID == 0)
            {
                return StatusCode.Success;
            }

            bool duplicateUsername = (this.genericRepository.GetSingle<Business.Member>(x =>
                                            x.Username == memberDTO.Username &&
                                            x.ID != memberDTO.ID) != null);
            if (duplicateUsername)
            {
                return StatusCode.DuplicateUsername;
            }

            bool duplicateEmail = (this.genericRepository.GetSingle<Business.Member>(x =>
                                            x.Email == memberDTO.Email &&
                                            x.ID != memberDTO.ID) != null);
            if (duplicateEmail)
            {
                return StatusCode.DuplicateEmail;
            }

            return StatusCode.Success ;
        }
        
        /// <summary>
        /// Constructs the Key, that is randomly generated from alphanumeric characters.
        /// Source of algorithm: <see href="http://stackoverflow.com/a/1344242/1027250" /> 
        /// </summary>
        /// <returns>
        /// Returns the Key.
        /// </returns>
        private string GenerateReferralCode()
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string referralCode = string.Empty;

            do
            {
                referralCode = new string(
                    Enumerable.Repeat(characters, ReferralCodeLength)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());
            }
            while (this.memberRepository.GetMemberByReferralCode(referralCode) != null);

            return referralCode;
        }

        private Business.Member GenerateReferrer(out StatusCode statusCode, string referrerUsername = null)
        {
            statusCode = StatusCode.Success;
            Business.Member member = null;
            if (!string.IsNullOrEmpty(referrerUsername))
            {
                member = this.memberRepository.GetMemberByUsername(referrerUsername);

                if (member == null)
                {
                    statusCode = StatusCode.ReferrerNotFound;
                }

                return member;
            }

            return this.memberRepository.GetRandomMember();
        }

    }
}
