namespace RedLions.Presentation.Web.Areas.Admin.Controllers
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
    
    [Authorize(Roles = "Admin")]
    public class MembersController : Controller
    {
        [Dependency]
        public MemberService MemberService { get; set; }

        //
        // GET: /Admin/Members/

        public ActionResult Index()
        {
            IEnumerable<DTO.Member> memberDTOs = this.MemberService.GetAllMembers();
            IEnumerable<Models.Member> memberModels = memberDTOs.Select(x => new Models.Member(x));
            return View(memberModels);
        }

        public ViewResult Create()
        {
            return View();
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
                Username = member.Username,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                ReferrerUsername = member.ReferrerUsername 
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
                ReferrerUsername = member.ReferrerUsername 
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
