﻿namespace RedLions.Presentation.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using RedLions.Presentation.Web.Components;
    using RedLions.Application;
    using DTO = RedLions.Application.DTO;

    [SaveReferrer]
    [Route("Home/{action=Index}/{referrerUsername?}", Order = 1)]
    public class HomeController : Controller
    {
        private MemberService memberService;
        private InquiryService inquiryService;

        public HomeController(
            InquiryService inquiryService,
            MemberService memberService)
        {
            this.memberService = memberService;
            this.inquiryService = inquiryService;
        }
        //
        // GET: /Home/

        [Route("Home/Index/{referrerUsername?}", Order = 1)]
        [Route("Home/{referrerUsername?}", Order = 2)]
        [Route("{referrerUsername?}", Order = 3)]
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Inquire(string referrerUsername = null)
        {
            return View();
        }

        [Route("Products/{referrerUsername?}")]
        public ViewResult Products()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Inquire(Models.Inquiry inquiry)
        {
            if (!ModelState.IsValid)
            {
                return View(inquiry);
            }

            var inquiryDTO = new DTO.Inquiry()
            {
                FirstName = inquiry.FirstName,
                LastName = inquiry.LastName,
                Email = inquiry.Email,
                CellphoneNumber = inquiry.CellphoneNumber,
                Message = inquiry.Message,
            };

            if (Session["ReferrerUsername"] != null)
            {
                string referrerUsername = Session["ReferrerUsername"] as string;
                DTO.Member referrer = this.memberService.GetMemberByUsername(referrerUsername);
                if (referrer != null)
                {
                    inquiryDTO.ReferrerID = referrer.ID;
                }
            }           

            this.inquiryService.SubmitInquiry(inquiryDTO);
            return View("InquireConfirm");
        }

        public PartialViewResult NavigationBar()
        {
            return PartialView("_NavigationPartial");
        }

        [Route("Awards/{referrerUsername?}")]
        public ViewResult Awards()
        {
            return View();
        }

        [Route("Certifications/{referrerUsername?}")]
        public ViewResult Certifications()
        {
            return View();
        }

        

        [Route("Redlions/{referrerUsername?}")]
        public ViewResult Redlions()
        {
            return View();
        }

        [Route("Beverages/{referrerUsername?}")]
        public ViewResult Beverages()
        {
            return View();
        }

        [Route("Nutritions/{referrerUsername?}")]
        public ViewResult Nutritions()
        {
            return View();
        }

        [Route("Personal/{referrerUsername?}")]
        public ViewResult Personal()
        {
            return View();
        }
                
        [Route("Health/{referrerUsername?}")]
        public ViewResult Health()
        {
            return View("UnderConstruction");
        }

        [Route("LocalBusiness/{referrerUsername?}")]
        public ViewResult LocalBusiness()
        {
            return View();
        }

        [Route("InternationalBusiness/{referrerUsername?}")]
        public ViewResult InternationalBusiness()
        {
            return View();
        }

        [Route("LocalPackages/{referrerUsername?}")]
        public ViewResult LocalPackages()
        {
            return View();
        }

        [Route("InternationalPackages/{referrerUsername?}")]
        public ViewResult InternationalPackages()
        {
            return View("UnderConstruction");
        }

        [Route("Dubai/{referrerUsername?}")]
        public ViewResult Dubai()
        {
            return View();
        }

        [Route("HongKong/{referrerUsername?}")]
        public ViewResult HongKong()
        {
            return View();
        }

        [Route("Macau/{referrerUsername?}")]
        public ViewResult Macau()
        {
            return View();
        }

        [Route("Ksa/{referrerUsername?}")]
        public ViewResult Ksa()
        {
            return View();
        }

        [Route("Singapore/{referrerUsername?}")]
        public ViewResult Singapore()
        {
            return View();
        }

        [Route("Featured/{referrerUsername?}")]
        public ViewResult Featured()
        {
            return View();
        }

        [Route("Featured/{id:int}/{referrerUsername?}", Name = "FeaturedSingle")]
        public ViewResult FeaturedSingle(int id)
        {
            switch(id)
            {
                case 1:
                    ViewBag.VideoID = "Cv8ExYqBB7A";
                    ViewBag.Title = "Go Negosyo with Ms. Mabel Gonzales";
                    break;  
                case 2:
                    ViewBag.VideoID = "HuMwSGUqJeg";
                    ViewBag.Title = "Go Negosyo Featured the 6th Anniversary Celebration of UNO";
                    break;
                case 3:
                    ViewBag.VideoID = "h7Ic7yjobhU";
                    ViewBag.Title = "Rated K";
                    break;
                default:
                    throw new HttpException(404, "Video not found");
            }

            return View();
        }

        [Route("Videos/{referrerUsername?}")]
        public ViewResult Videos()
        {
            return View();
        }

        [Route("Possible/{referrerUsername?}")]
        public ViewResult Possible()
        {
            return View();
        }

        [Route("Membership/{referrerUsername?}")]
        public ViewResult Membership()
        {
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem()
                {
                    Value = "1",
                    Text = "Local",
                });

            selectListItems.Add(new SelectListItem()
                {
                    Value = "2",
                    Text = "International",
                });

            var viewModel = new ViewModels.Membership()
            {
                LocationSelectListItems = selectListItems,
            };

            return View(viewModel);
        }

        [Route("Company/{id:int}/{referrerUsername?}")]
        public ViewResult Company(int id)
        {
            switch(id)
            {
                case 1:
                    ViewBag.Company = true;
                    ViewBag.Board = true;
                    ViewBag.MissionVision = true;
                    ViewBag.Certifications = true;
                    ViewBag.Awards = true;
                    break;
                case 2:
                    ViewBag.Company = true;
                    break;
                case 3:
                    ViewBag.Board = true;
                    break;
                case 4:
                    ViewBag.MissionVision = true;
                    break;
                case 5:
                    ViewBag.Certifications = true;
                    break;
                case 6:
                    ViewBag.Awards = true;
                    break;
                default:
                    throw new HttpException("Incorrect ID in company page.");
            }

            return View();
        }

        [Route("Organization/{id:int}/{referrerUsername?}")]
        public ViewResult Organization(int id)
        {
            switch (id)
            {
                case 1:
                    ViewBag.Who = true;
                    ViewBag.History = true;
                    ViewBag.Founder = true;
                    break;
                case 2:
                    ViewBag.Who = true;
                    break;
                case 3:
                    ViewBag.History = true;
                    break;
                case 4:
                    ViewBag.Founder = true;
                    break;
                default:
                    throw new HttpException("Incorrect ID in organization page.");
            }

            return View();
        }

        [Route("Skin/{referrerUsername?}")]
        public ViewResult Skin()
        {
            return View();
        }

        [Route("ProductDetails/{id:int}/{referrerUsername?}", Name = "ProductDetails")]
        public ViewResult ProductDetails(int id)
        {
            string viewName = string.Empty;
            switch (id)
            {
                case 1:
                    viewName = "BbCream"; 
                    break;
                case 2:
                    viewName = "Body";
                    break;
                case 3:
                    viewName = "GlutaLotion";
                    break;
                case 4:
                    viewName = "GlutaCapsule";
                    break;
                case 5:
                    viewName = "GlutaSoap";
                    break;
                case 6:
                    viewName = "Kojic";
                    break;
                case 7:
                    viewName = "Magic";
                    break;
                case 8:
                    viewName = "Thana";
                    break;
                case 9:
                    viewName = "Fit";
                    break;
                case 10:
                    viewName = "Grape";
                    break;
                case 11:
                    viewName = "Krypt";
                    break;
                case 12:
                    viewName = "Super";
                    break;
                case 13:
                    viewName = "Ultima";
                    break;
                case 14:
                    viewName = "Vco";
                    break;
                case 15:
                    viewName = "Wheat";
                    break;
                default:
                    throw new HttpException(404, "Product not found.");
            }

            return View(viewName);
        }

        [Route("Food/{referrerUsername?}")]
        public ViewResult Food()
        {
            return View();
        }

        // This is for updating the login section in the homepage after the user logins in.
        public PartialViewResult AccountSection()
        {
            return PartialView("_AccountPartial");
        }

        [Route("International/{referrerUsername?}")]
        public ViewResult International()
        {
            return View();
        }
    }
}
