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
        private IUnitOfWork unitOfWork;
        private IRepository genericRepository;
        private IMemberRepository memberRepository;
        private IUserRepository userRepository;
        private ICountryRepository countryRepository;
        private ISubscriptionRepository subscriptionRepository;

        public MemberService(
            IUnitOfWork unitOfWork,
            IRepository genericRepository,
            IUserRepository userRepository,
            IMemberRepository memberRepository,
            ICountryRepository countryRepository,
            ISubscriptionRepository subscriptionRepository)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }

            if (memberRepository == null)
            {
                throw new ArgumentNullException("memberRepository");
            }

            if (genericRepository == null)
            {
                throw new ArgumentNullException("memberRepository");
            }

            if (countryRepository == null)
            {
                throw new ArgumentNullException("countryRepository");
            }

            if (subscriptionRepository == null)
            {
                throw new ArgumentNullException("subscriptionRepository");
            }

            this.unitOfWork = unitOfWork;
            this.genericRepository = genericRepository;
            this.userRepository = userRepository;
            this.memberRepository = memberRepository;
            this.countryRepository = countryRepository;
            this.subscriptionRepository = subscriptionRepository;
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

        public DTO.Member GetRandomMember()
        {
            Business.Member randomMember = this.memberRepository.GetRandomMember();

            return MemberAssembler.ToDTO(randomMember);
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
            statusCode = this.Validate(memberDTO);
            if (statusCode != StatusCode.Success)
            {
                return statusCode;
            }

            Business.Inquiry inquiry = null;
            if (memberDTO.InquiryID.HasValue)
            {
                inquiry = this.genericRepository.GetById<Business.Inquiry>(memberDTO.InquiryID.Value);
            }

            Business.Country country = this.countryRepository.GetByID(memberDTO.Country.ID);

            int silverSubscriptionID = 1;
            Business.Subscription subscription = this.subscriptionRepository.GetByID(silverSubscriptionID);

            var member = new Business.Member(
                inquiry: inquiry,
                username: memberDTO.Username,
                firstName: memberDTO.FirstName,
                lastName: memberDTO.LastName,
                email: memberDTO.Email,
                personalReferralCode: this.GenerateReferralCode(),
                cellphoneNumber: memberDTO.CellphoneNumber,
                homeAddress: memberDTO.HomeAddress,
                deliveryAddress: memberDTO.DeliveryAddress,
                nationality: memberDTO.Nationality,
                subscription: subscription,
                country: country,
                unoID: memberDTO.UnoID);

            member.Referrer = this.GenerateReferrer(out statusCode, memberDTO.ReferrerUsername);

            if (statusCode == StatusCode.Success)
            {
                this.memberRepository.RegisterMember(member);
                this.unitOfWork.Commit();
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
            member.UnoID = memberDTO.UnoID;
            member.DeliveryAddress = memberDTO.DeliveryAddress;
            member.HomeAddress = memberDTO.HomeAddress;
            member.Nationality = memberDTO.Nationality;

            if (member.Country.ID != memberDTO.Country.ID)
            {
                Business.Country country = this.countryRepository.GetByID(memberDTO.Country.ID);
                member.Country = country;
            }

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

        public StatusCode ResetPassword(int userID)
        {
            var statusCode = StatusCode.Success;

            Business.Member member = this.memberRepository.GetMemberByID(userID);

            if (member == null)
            {
                throw new Exception("Unable to reset password, user not found");
            }

            member.ResetPassword();
            this.unitOfWork.Commit();

            return statusCode;
        }

        public void UpdatePoints(int adminUserID, int memberUserID, int points)
        {
            // Get and verify adminUserID
            Business.User adminUser = this.userRepository.GetUserByID(adminUserID);
            if (adminUser == null)
            {
                throw new Exception("Unable to update points, admin's user ID was not found.");
            }

            // Retrieve and verify memberUserID
            Business.Member member = this.memberRepository.GetMemberByID(memberUserID);
            if (member == null)
            {
                throw new Exception("Unable to update points, member's user ID was not found.");
            }

            // Update points
            member.AddPoints(adminUser, points);

            this.unitOfWork.Commit();
        }

        public void Deactivate(int userID)
        {
            Business.Member member = this.memberRepository.GetMemberByID(userID);
            if (member == null)
            {
                throw new Exception("Unable to deactivate account, the user was not found.");
            }

            member.Deactivate();
            this.unitOfWork.Commit();
        }

        public void Activate(int userID)
        {
            Business.Member member = this.memberRepository.GetMemberByID(userID);
            if (member == null)
            {
                throw new Exception("Unable to deactivate account, the user was not found.");
            }

            member.Activate();
            this.unitOfWork.Commit();
        }

        private StatusCode Validate(DTO.Member memberDTO)
        {
            if (memberDTO == null)
            {
                throw new ArgumentNullException("memberDTO");
            }

            bool duplicateUsername = (this.genericRepository.GetSingle<Business.Member>(x =>
                                            x.Username == memberDTO.Username &&
                                            x.ID != memberDTO.ID) != null);
            if (duplicateUsername)
            {
                return StatusCode.DuplicateUsername;
            }

            bool usernameInvalid = !memberDTO.Username.All(c => Char.IsLetterOrDigit(c) || c == '_');
            if (usernameInvalid)
            {
                return StatusCode.UsernameInvalid;
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
        /// <remarks>
        /// Q: Why is this here, and not in the member entity (domain)?
        /// A: Because we need to verify that the generated code has no conflicts with other
        /// members, which can only be done outside of the domain. Unless Members has a parent or
        /// we create a factory.
        /// </remarks>
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
