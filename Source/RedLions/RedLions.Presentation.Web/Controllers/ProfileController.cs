namespace RedLions.Presentation.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    // Third Party
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

            StatusCode statusCode = this.MemberService.Update(memberDTO);

            if (statusCode != StatusCode.Success)
            {
                this.AddError(statusCode);
                return View(member);
            }

            return RedirectToAction("Index");
        }

        public ViewResult Referrals()
        {
            DTO.Member memberDTO = this.GetMemberDTO();
            IEnumerable<DTO.Member> memberDTOs = this.MemberService.GetReferrals(memberDTO.ID);
            IEnumerable<Models.Member> memberModels = memberDTOs.Select(x => new Models.Member(x));

            return View(memberModels);
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
