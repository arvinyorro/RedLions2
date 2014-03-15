namespace RedLions.Presentation.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    // Third Party
    using PagedList;
    using Microsoft.Practices.Unity;
    // Other Layers
    using RedLions.Application;
    using DTO = RedLions.Application.DTO;
    // Internal
    using RedLions.Presentation.Web.Components;

    [Authorize(Roles = "Member")]
    public class ProfileController : Controller
    {
        private MemberService memberService;
        public UserService userService;
        public InquiryService inquiryService;

        public ProfileController(
            MemberService memberService,
            UserService userService,
            InquiryService inquiryService)
        {
            this.memberService = memberService;
            this.userService = userService;
            this.inquiryService = inquiryService;
        }

        //
        // GET: /Admin/Home/

        public ActionResult Index()
        {
            string username = User.Identity.Name;
            DTO.Member memberDTO = this.memberService.GetMemberByUsername(username);
            Models.Member memberModel = new Models.Member(memberDTO);
            return View(memberModel);
        }

        public ViewResult Edit()
        {
            DTO.Member memberDTO = this.GetMemberDTO();
            ViewModels.EditMember editMemberViewModel = new ViewModels.EditMember(memberDTO);
            return View(editMemberViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ViewModels.EditMember member)
        {
            if (!ModelState.IsValid)
            {
                return View(member);
            }

            string username = User.Identity.Name;
            DTO.Member memberDTO = this.memberService.GetMemberByUsername(username);

            memberDTO.Username = member.Username;
            memberDTO.FirstName = member.FirstName;
            memberDTO.LastName = member.LastName;
            memberDTO.Email = member.Email;
            memberDTO.CellphoneNumber = member.CellphoneNumber;

            StatusCode statusCode = this.memberService.Update(memberDTO);

            if (statusCode != StatusCode.Success)
            {
                this.AddError(statusCode);
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
