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
    using RedLions.Presentation.Web.ViewModels;
    
    [Authorize(Roles = "Admin")]
    public class MembersController : Controller
    {
        [Dependency]
        public MemberService MemberService { get; set; }

        [Dependency]
        public InquiryService InquiryService { get; set; }

        [Dependency]
        public CountryService CountryService { get; set; }

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

            var templateMember = new UpdateMember();

            if (inquiryID.HasValue ||
                inquiryDTO != null)
            {
                templateMember.InquiryID = inquiryDTO.ID;
                templateMember.Email = inquiryDTO.Email;
                templateMember.FirstName = inquiryDTO.FirstName;
                templateMember.LastName = inquiryDTO.LastName;
                templateMember.ReferrerUsername = inquiryDTO.ReferrerUsername;
                templateMember.CellphoneNumber = inquiryDTO.CellphoneNumber;
            }

            // Country.
            DTO.Country defaultCountryDTO = this.CountryService.GetDefaultCountry();
            templateMember.Country = new Models.Country(defaultCountryDTO);
            templateMember.CountrySelectListItems = this.CountryService
                .GetAll()
                .Select(x => new Models.Country(x))
                .ToSelectListItems(defaultCountryDTO.ID);

            return View(templateMember);
        }

        [HttpPost]
        public ActionResult Create(UpdateMember member)
        {
            if (!ModelState.IsValid)
            {
                member.CountrySelectListItems = this.CountryService
                    .GetAll()
                    .Select(x => new Models.Country(x))
                    .ToSelectListItems(member.Country.ID); 

                return View(member);
            }

            if (member.Username.ToLower() == "profile")
            {
                ModelState.AddModelError("Username", "Profile username is a reserved. Please use a different username.");

                member.CountrySelectListItems = this.CountryService
                    .GetAll()
                    .Select(x => new Models.Country(x))
                    .ToSelectListItems(member.Country.ID); 

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
                UnoID = member.UnoID,
                Country = new DTO.Country() {  ID = member.Country.ID },
            };

            StatusCode statusCode = this.MemberService.Register(memberDTO);

            if (statusCode != StatusCode.Success)
            {
                this.AddError(statusCode);

                member.CountrySelectListItems = this.CountryService
                    .GetAll()
                    .Select(x => new Models.Country(x))
                    .ToSelectListItems(member.Country.ID); 
                return View(member);
            }

            return RedirectToAction("Index");
        }

        public ViewResult Details(int userID)
        {
            DTO.Member memberDTO = this.MemberService.GetMemberByID(userID);
            var memberModel = new Models.Member(memberDTO);

            return View(memberModel);
        }

        public ViewResult Edit(int userID)
        {
            DTO.Member memberDTO = this.MemberService.GetMemberByID(userID);
            IEnumerable<Models.Country> countryModels = this.CountryService
                    .GetAll()
                    .Select(x => new Models.Country(x));

            var memberModel = new UpdateMember(memberDTO, countryModels);

            return View(memberModel);
        }

        [HttpPost]
        public ActionResult Edit(UpdateMember member)
        {
            if (!ModelState.IsValid)
            {
                member.CountrySelectListItems = this.CountryService
                    .GetAll()
                    .Select(x => new Models.Country(x))
                    .ToSelectListItems(member.Country.ID); 

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
                CellphoneNumber = member.CellphoneNumber,
                UnoID = member.UnoID,
                Country = new DTO.Country() { ID = member.Country.ID },
            };

            StatusCode statusCode = this.MemberService.Update(memberDTO);

            if (statusCode != StatusCode.Success)
            {
                this.AddError(statusCode);
                member.CountrySelectListItems = this.CountryService
                    .GetAll()
                    .Select(x => new Models.Country(x))
                    .ToSelectListItems(member.Country.ID); 

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
                case StatusCode.UsernameInvalid:
                    ModelState.AddModelError("Username", errorMessage);
                    break;
                case StatusCode.DuplicateEmail:
                    ModelState.AddModelError("Email", errorMessage);
                    break;
            }
        }
    }
}
