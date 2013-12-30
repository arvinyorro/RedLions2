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
        [Dependency]
        public MemberService MemberService { get; set; }

        [Dependency]
        public UserService UserService { get; set; }

        //
        // GET: /Admin/Home/

        public ActionResult Index()
        {
            string username = User.Identity.Name;
            DTO.Member memberDTO = this.MemberService.GetMemberByUsername(username);
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
            DTO.Member memberDTO = this.MemberService.GetMemberByUsername(username);

            memberDTO.Username = member.Username;
            memberDTO.FirstName = member.FirstName;
            memberDTO.LastName = member.LastName;
            memberDTO.Email = member.Email;
            memberDTO.CellphoneNumber = member.CellphoneNumber;

            StatusCode statusCode = this.MemberService.Update(memberDTO);

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

            IEnumerable<Models.Member> memberModels = this.MemberService
                .GetReferrals(currentPage, out totalReferrals, memberDTO.ID)
                .ToModels();

            int pageSize = this.MemberService.PageSize;

            var pagedList = new StaticPagedList<Models.Member>(
                memberModels,
                currentPage,
                pageSize,
                totalReferrals);

            return View(pagedList);
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
            StatusCode statusCode = this.UserService.ChangePassword(
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
            return this.MemberService.GetMemberByUsername(username);
        }

        private void AddError(StatusCode statusCode)
        {
            string errorMessage = StatusCodeHandler.ErrorCodeToString(statusCode);

            switch (statusCode)
            {
                case StatusCode.PasswordInvalid:
                    ModelState.AddModelError("OldPassword", errorMessage);
                    break;
            }
        }

    }
   
}
