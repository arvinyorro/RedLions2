namespace RedLions.Presentation.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    // Third Party
    using Microsoft.Practices.Unity;
    using PagedList;
    using AutoMapper;
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
        public UserService UserService { get; set; }

        [Dependency]
        public MemberService MemberService { get; set; }

        [Dependency]
        public InquiryService InquiryService { get; set; }

        [Dependency]
        public CountryService CountryService { get; set; }

        [Dependency]
        public SubscriptionService SubscriptionService { get; set; }

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

        public ViewResult Details(int id)
        {
            DTO.Member memberDTO = this.MemberService.GetMemberByID(id);
            var memberModel = new Models.Member(memberDTO);

            return View(memberModel);
        }

        public ViewResult Edit(int id)
        {
            DTO.Member memberDTO = this.MemberService.GetMemberByID(id);
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
                DeliveryAddress = member.DeliveryAddress,
                HomeAddress = member.HomeAddress,
                Nationality = member.Nationality,
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

            return RedirectToAction("Details", new { id = memberDTO.ID });
        }

        public ViewResult ResetPassword(int id)
        {
            DTO.Member memberDTO = this.MemberService.GetMemberByID(id);

            if (memberDTO == null)
            {
                throw new Exception("User not found.");
            }

            ViewBag.UserID = id;

            return View();
        }

        [HttpPost, ActionName("ResetPassword")]
        public ActionResult ResetPasswordConfirm(int id)
        {
            this.MemberService.ResetPassword(id);

            ViewBag.UserID = id;

            return View("ResetPasswordConfirmed");
        }

        public ViewResult Subscription(int id)
        {
            DTO.Member memberDTO = this.MemberService.GetMemberByID(id);
            IEnumerable<DTO.Subscription> subscriptionDTOList = this.SubscriptionService.GetSubscriptions();
            IEnumerable<Models.Subscription> subscriptionModels = Mapper.Map<IEnumerable<Models.Subscription>>(subscriptionDTOList);

            var viewModel = new UpdateSubscription(memberDTO, subscriptionModels);
            return View(viewModel);
        }

        [HttpPost, ActionName("Subscription")]
        public ViewResult SubscriptionConfirm(int userID, int subscriptionID)
        {
            this.SubscriptionService.ExtendSubscription(userID, subscriptionID);
            ViewBag.UserID = userID;
            return View("SubscriptionConfirmed");
        }

        public ViewResult Points(int id)
        {
            var viewModel = new ViewModels.MemberUpdatePoints()
            {
                MemberUserID = id,
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Points(ViewModels.MemberUpdatePoints memberUpdatePoints)
        {
            if(!ModelState.IsValid)
            {
                return View(memberUpdatePoints);
            }
            DTO.User adminDTO = this.UserService.GetUserByUsername(User.Identity.Name);

            this.MemberService.UpdatePoints(adminDTO.ID, 
                memberUpdatePoints.MemberUserID, 
                memberUpdatePoints.Points);

            return RedirectToAction("Details", new { id = memberUpdatePoints.MemberUserID });
        }

        public ViewResult Activation(int id)
        {
            DTO.Member memberDTO = this.MemberService.GetMemberByID(id);

            ViewBag.Deactivated = memberDTO.Deactivated;
            ViewBag.UserID = id;
            return View();
        }

        [HttpPost, ActionName("Activation")]
        public ActionResult ToggleActivation(int userID)
        {
            DTO.Member memberDTO = this.MemberService.GetMemberByID(userID);

            if (memberDTO.Deactivated)
            {
                this.MemberService.Activate(userID);
            }
            else
            {
                this.MemberService.Deactivate(userID);
            }
            

            return RedirectToAction("Details", new { id = userID });
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
