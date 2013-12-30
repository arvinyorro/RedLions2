namespace RedLions.Presentation.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    // Third Party
    using Microsoft.Practices.Unity;
    using PagedList;
    // Other Layers
    using RedLions.Application;
    using DTO = RedLions.Application.DTO;
    // Internal
    using RedLions.Presentation.Web.Components;
    
    [Authorize(Roles = "Admin")]
    public class MembersController : Controller
    {
        [Dependency]
        public MemberService MemberService { get; set; }

        [Dependency]
        public InquiryService InquiryService { get; set; }

        //
        // GET: /Admin/Members/

        public ActionResult Index(int? page, string searchUsername)
        {
            ViewBag.SearchUsername = searchUsername;

            int currentPage = (page ?? 1);

            // Fix negative page
            currentPage = currentPage < 0 ? 1 : currentPage;

            int totalItems = 0;
            
            IEnumerable<DTO.Member> memberDTOs = this.MemberService.GetPagedMembers(currentPage, out totalItems, searchUsername);          
            IEnumerable<Models.Member> memberModels = memberDTOs.Select(x => new Models.Member(x));

            int pageSize = this.MemberService.PageSize;

            var pagedList = new StaticPagedList<Models.Member>(memberModels, currentPage, pageSize, totalItems);

            return View(pagedList);
        }

        public ViewResult Create(int? inquiryID)
        {
            DTO.Inquiry inquiryDTO = null;
            if (inquiryID.HasValue)
            {
                inquiryDTO = this.InquiryService.GetById(inquiryID.Value);
            }

            if (!inquiryID.HasValue ||
                inquiryDTO == null)
            {
                return View();
            }

            var templateMember = new Models.Member();
            templateMember.InquiryID = inquiryDTO.ID;
            templateMember.Email = inquiryDTO.Email;
            templateMember.FirstName = inquiryDTO.FirstName;
            templateMember.LastName = inquiryDTO.LastName;
            templateMember.ReferrerUsername = inquiryDTO.ReferrerUsername;
            templateMember.CellphoneNumber = inquiryDTO.CellphoneNumber;

            return View(templateMember);
        }

        [HttpPost]
        public ActionResult Create(Models.Member member)
        {
            if (!ModelState.IsValid)
            {
                return View(member);
            }

            var memberDTO = new DTO.Member()
            {
                InquiryID = member.InquiryID,
                Username = member.Username,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                ReferrerUsername = member.ReferrerUsername,
                CellphoneNumber = member.CellphoneNumber,
            };

            this.MemberService.Register(memberDTO);

            return RedirectToAction("Index");
        }

        public ViewResult Edit(int userID)
        {
            DTO.Member memberDTO = this.MemberService.GetMemberByID(userID);
            var memberModel = new ViewModels.EditMember(memberDTO);

            return View(memberModel);
        }

        [HttpPost]
        public ActionResult Edit(ViewModels.EditMember member)
        {
            if (!ModelState.IsValid)
            {
                return View(member);
            }

            var memberDTO = new DTO.Member()
            {
                ID = member.ID,
                Username = member.Username,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                ReferrerUsername = member.ReferrerUsername,
                CellphoneNumber = member.CellphoneNumber
            };

            StatusCode statusCode = this.MemberService.Update(memberDTO);

            if (statusCode != StatusCode.Success)
            {
                this.AddError(statusCode);
                return View(member);
            }

            return RedirectToAction("Index");
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
                    ModelState.AddModelError("Username", errorMessage);
                    break;
                case StatusCode.DuplicateEmail:
                    ModelState.AddModelError("Email", errorMessage);
                    break;
            }
        }
    }
}
