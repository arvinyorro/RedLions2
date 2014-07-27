namespace RedLions.Presentation.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    // Third Party
    using PagedList;
    using Microsoft.Practices.Unity;
    using AutoMapper;
    // Other Layers
    using RedLions.Application;
    using DTO = RedLions.Application.DTO;
    // Internal
    using RedLions.Presentation.Web.Components;
    using RedLions.Presentation.Web.ViewModels;

    [Authorize(Roles = "Member")]
    [CheckSubscriptionExpiration]
    public class ProfileController : Controller
    {
        private MemberService memberService;
        public UserService userService;
        public InquiryService inquiryService;
        private CountryService countryService;
        private InquiryChatService inquiryChatService;
        private PaymentService paymentService;

        public ProfileController(
            MemberService memberService,
            UserService userService,
            InquiryService inquiryService,
            CountryService countryService,
            InquiryChatService inquiryChatService,
            PaymentService paymentService)
        {
            this.memberService = memberService;
            this.userService = userService;
            this.inquiryService = inquiryService;
            this.countryService = countryService;
            this.inquiryChatService = inquiryChatService;
            this.paymentService = paymentService;
        }

        //
        // GET: /Admin/Home/

        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Details()
        {
            //  Can be refactored, see GetMemberDTO().
            string username = User.Identity.Name;
            DTO.Member memberDTO = this.memberService.GetMemberByUsername(username);
            Models.Member memberModel = new Models.Member(memberDTO);
            return View(memberModel);
        }

        public ViewResult Edit()
        {
            DTO.Member memberDTO = this.GetMemberDTO();
            IEnumerable<Models.Country> countryModels = this.countryService
                .GetAll()
                .Select(x => new Models.Country(x));
            ViewModels.UpdateMember editMemberViewModel = new UpdateMember(memberDTO, countryModels);
            return View(editMemberViewModel);
        }

        [HttpPost]
        public ActionResult Edit(UpdateMember member)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<Models.Country> countryModels = this.countryService
                    .GetAll()
                    .Select(x => new Models.Country(x));
                member.CountrySelectListItems = countryModels.ToSelectListItems(member.Country.ID);
                return View(member);
            }

            string username = User.Identity.Name;
            DTO.Member memberDTO = this.memberService.GetMemberByUsername(username);

            memberDTO.Username = member.Username;
            memberDTO.FirstName = member.FirstName;
            memberDTO.LastName = member.LastName;
            memberDTO.Email = member.Email;
            memberDTO.CellphoneNumber = member.CellphoneNumber;
            memberDTO.Country.ID = member.Country.ID;
            memberDTO.UnoID = member.UnoID;

            StatusCode statusCode = this.memberService.Update(memberDTO);

            if (statusCode != StatusCode.Success)
            {
                this.AddError(statusCode);
                IEnumerable<Models.Country> countryModels = this.countryService
                    .GetAll()
                    .Select(x => new Models.Country(x));
                member.CountrySelectListItems = countryModels.ToSelectListItems(member.Country.ID);
                return View(member);
            }

            return RedirectToAction("Index");
        }

        public ViewResult Referrals(int? page)
        {
            int currentPage = (page ?? 1);

            // Fix negative page
            currentPage = currentPage < 0 ? 1 : currentPage;

            int totalReferrals = 0;

            DTO.Member memberDTO = this.GetMemberDTO();

            IEnumerable<Models.Member> memberModels = this.memberService
                .GetReferrals(currentPage, out totalReferrals, memberDTO.ID)
                .ToModels();

            int pageSize = this.memberService.PageSize;

            var pagedList = new StaticPagedList<Models.Member>(
                memberModels,
                currentPage,
                pageSize,
                totalReferrals);

            return View(pagedList);
        }

        public ViewResult Inquiries(int? page, string searchEmail)
        {
            ViewBag.SearchEmail = searchEmail;

            int currentPage = (page ?? 1);
            int totalInquiries = 0;
            DTO.Member memberDTO = this.GetMemberDTO();

            IEnumerable<Models.Inquiry> inquiryModels = this.inquiryService
                .GetPagedInquiriesByMember(
                    memberDTO.ID,
                    currentPage,
                    out totalInquiries,
                    searchEmail)
                .Select(x => new Models.Inquiry(x));

            int pageSize = this.inquiryService.PageSize;

            var pagedList = new StaticPagedList<Models.Inquiry>(inquiryModels, currentPage, pageSize, totalInquiries);
            return View(pagedList);
        }

        public ViewResult Inquiry(int id)
        {
            DTO.Inquiry inquiryDTO = this.inquiryService.GetById(id);
            var inquiry = new Models.Inquiry(inquiryDTO);
            return View(inquiry);
        }

        public ViewResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(Models.ChangePassword changePassword)
        {
            if (!ModelState.IsValid)
            {
                return View(changePassword);
            }

            DTO.Member memberDTO = this.GetMemberDTO();
            StatusCode statusCode = this.userService.ChangePassword(
                    memberDTO.ID, 
                    changePassword.OldPassword, 
                    changePassword.NewPassword);

            if (statusCode != StatusCode.Success)
            {
                this.AddError(statusCode);
                return View(changePassword);
            }

            return RedirectToAction("Index");
        }

        public ViewResult Messages()
        {
            // Retrieve user details.
            DTO.Member memberDTO = this.memberService.GetMemberByUsername(User.Identity.Name);

            // Retrieve sessions.
            IEnumerable<DTO.InquiryChatSession> chatSessionDTOList = this.inquiryChatService
                .GetSessionsByMember(memberDTO.ID);

            // Map DTO list to Models.
            IEnumerable<Models.InquiryChatSession> chatSessions = Mapper
                .Map<IEnumerable<Models.InquiryChatSession>>(chatSessionDTOList);

            // Create View Model
            var viewModel = new ViewModels.MemberMessagesViewModel()
            {
                ChatSessions = Mapper.Map<IEnumerable<Models.InquiryChatSession>>(chatSessionDTOList),
                Username = memberDTO.Username,
            };

            // Get default session.
            if (chatSessions.Any())
            {
                viewModel.SelectedChatSessionID = chatSessions.First().ID;
            }

            // Create Chat Message for Page
            var chatMessage = new Models.InquiryChatMessage()
            {
                InquiryChatSessionID = viewModel.SelectedChatSessionID,
                Name = string.Format("{0} {1}",memberDTO.FirstName, memberDTO.LastName),
            };
            viewModel.ChatMessage = chatMessage;

            return View(viewModel);
        }

        public ViewResult Payments(int? page, string searchEmail, bool unread = false)
        {
            ViewBag.SearchEmail = searchEmail;
            ViewBag.UnreadOnly = unread;

            int currentPage = (page ?? 1);

            // Fix negative page
            currentPage = currentPage < 0 ? 1 : currentPage;

            int pageSize = 20;
            int totalItems = 0;

            DTO.Member memberDTO = this.GetMemberDTO();

            IEnumerable<DTO.Payment> paymentDtoList = this.paymentService
                .GetPagedListByMember(
                    memberDTO.ID,
                    currentPage,
                    out totalItems,
                    pageSize,
                    searchEmail,
                    unread);

            int totalUnread = this.paymentService.GetUnreadCountByMember(memberDTO.ID);
            ViewBag.TotalUnread = totalUnread;

            IEnumerable<Models.Payment> announcementModels = Mapper.Map<IEnumerable<Models.Payment>>(paymentDtoList);

            var pagedList = new StaticPagedList<Models.Payment>(announcementModels, currentPage, pageSize, totalItems);

            return View(pagedList);
        }

        public ActionResult ReadAllPayments()
        {
            DTO.Member memberDTO = this.GetMemberDTO();
            this.paymentService.ReadAllPaymentsByMember(memberDTO.ID);
            return RedirectToAction("Payments");
        }

        public ViewResult Payment(int id)
        {
            DTO.Payment paymentDto = this.paymentService.GetByID(id);
            Models.Payment paymentModel = Mapper.Map<Models.Payment>(paymentDto);

            this.paymentService.ReadPaymentByMember(id);

            return View(paymentModel);
        }

        private DTO.Member GetMemberDTO()
        {
            string username = User.Identity.Name;
            return this.memberService.GetMemberByUsername(username);
        }

        private void AddError(StatusCode statusCode)
        {
            string errorMessage = StatusCodeHandler.ErrorCodeToString(statusCode);

            switch (statusCode)
            {
                case StatusCode.ReferrerNotFound:
                    ModelState.AddModelError("ReferrerUsername", errorMessage);
                    break;
                case StatusCode.DuplicateUsername:
                case StatusCode.UsernameInvalid:
                    ModelState.AddModelError("Username", errorMessage);
                    break;
                case StatusCode.DuplicateEmail:
                    ModelState.AddModelError("Email", errorMessage);
                    break;
                case StatusCode.PasswordInvalid:
                    ModelState.AddModelError("OldPassword", errorMessage);
                    break;
            }
        }
    }
   
}
