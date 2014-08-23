namespace RedLions.Presentation.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using RedLions.Presentation.Web.Components;
    using RedLions.Application;
    using DTO = RedLions.Application.DTO;
    using AutoMapper;
    using PagedList;

    [SaveReferrer]
    [Route("Home/{action=Index}/{referrerUsername?}", Order = 1)]
    public class HomeController : Controller
    {
        private MemberService memberService;
        private InquiryService inquiryService;
        private AnnouncementService announcementService;

        public HomeController(
            InquiryService inquiryService,
            MemberService memberService,
            AnnouncementService announcementService)
        {
            this.memberService = memberService;
            this.inquiryService = inquiryService;
            this.announcementService = announcementService;
        }

        //
        // GET: /Home/

        [Route("Home/Index/{referrerUsername?}", Order = 1)]
        [Route("Home/{referrerUsername?}", Order = 2)]
        [Route("{referrerUsername?}", Order = 3)]
        public ActionResult Index(string referrerUsername = null)
        {
            ViewBag.ReferrerUsername = referrerUsername;
            return View();
        }

        [Route("Inquire/{referrerUsername?}")]
        public ViewResult Inquire(string referrerUsername = null)
        {
            ViewBag.ReferrerUsername = referrerUsername;
            return View();
        }

        [Route("Products/{prod:int}/{referrerUsername?}")]
        public ViewResult Products(int prod, string referrerUsername = null)
        {
            ViewBag.SelectedID = prod;
            ViewBag.ReferrerUsername = referrerUsername;
            return View();
        }

        [HttpPost]
        public ActionResult InquireConfirm(Models.Inquiry inquiry)
        {
            if (!ModelState.IsValid)
            {
                return View("Inquire", inquiry);
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

        public ViewResult Expired()
        {
            return View();
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
        public ViewResult Membership(string referrerUsername = null)
        {
            ViewBag.ReferrerUsername = referrerUsername;

            var locationItems = new List<SelectListItem>();

            locationItems.Add(new SelectListItem()
            {
                Value = "1",
                Text = "International",
            });

            locationItems.Add(new SelectListItem()
                {
                    Value = "2",
                    Text = "Local",
                });

            

            var localItems = new List<SelectListItem>();
            localItems.Add(new SelectListItem()
            {
                Value = "1",
                Text = "Luzon",
            });

            localItems.Add(new SelectListItem()
            {
                Value = "2",
                Text = "Visayas",
            });

            localItems.Add(new SelectListItem()
            {
                Value = "3",
                Text = "Mindanao",
            });


            var viewModel = new ViewModels.Membership()
            {
                LocationItems = locationItems,
                LocalItems = localItems,
            };

            return View(viewModel);
        }

        [Route("Company/{comp:int}/{referrerUsername?}")]
        public ViewResult Company(int comp, string referrerUsername = null)
        {
            switch (comp)
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

            ViewBag.ReferrerUsername = referrerUsername;
            return View();
        }

        [Route("Organization/{org:int}/{referrerUsername?}")]
        public ViewResult Organization(int org, string referrerUsername = null)
        {
            switch (org)
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

            ViewBag.ReferrerUsername = referrerUsername;
            return View();
        }

        [Route("Skin/{referrerUsername?}")]
        public ViewResult Skin()
        {
            return View();
        }

        [Route("ProductDetails/{id:int}/{referrerUsername?}", Name = "ProductDetails")]
        public ViewResult ProductDetails(int id, string referrerUsername = null)
        {
            ViewBag.ReferrerUsername = referrerUsername;
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

        [Route("International/{id:int}/{referrerUsername?}")]
        public ViewResult International(int id, string referrerUsername = null)
        {
            ViewBag.ReferrerUsername = referrerUsername;
            ViewBag.SelectedID = id;
            return View();
        }

        [Route("LocalPlan/{referrerUsername?}")]
        public ViewResult LocalPlan(string referrerUsername = null)
        {
            ViewBag.ReferrerUsername = referrerUsername;
            var localItems = new List<SelectListItem>();
            localItems.Add(new SelectListItem()
            {
                Value = "1",
                Text = "Luzon",
            });

            localItems.Add(new SelectListItem()
            {
                Value = "2",
                Text = "Visayas",
            });

            localItems.Add(new SelectListItem()
            {
                Value = "3",
                Text = "Mindanao",
            });


            var viewModel = new ViewModels.Membership()
            {
                LocalItems = localItems,
            };

            return View(viewModel);
        }

        [Route("InternationalPlan/{referrerUsername?}")]
        public ViewResult InternationalPlan(string referrerUsername = null)
        {
            ViewBag.ReferrerUsername = referrerUsername;
            return View();
        }

        [Route("Announcements/{page:int}/{referrerUsername?}")]
        public ViewResult Announcements(int page, string referrerUsername = null)
        {
            ViewBag.ReferrerUsername = referrerUsername;

            // Fix negative page
            int currentPage = page < 0 ? 1 : page;

            int pageSize = 20;
            int totalItems = 0;

            IEnumerable<DTO.Announcement> announcementDtoList = this.announcementService
                .GetPagedAnnouncements(
                    currentPage,
                    out totalItems,
                    pageSize);

            IEnumerable<Models.Announcement> announcementModels = Mapper.Map<IEnumerable<Models.Announcement>>(announcementDtoList);

            var pagedList = new StaticPagedList<Models.Announcement>(announcementModels, currentPage, pageSize, totalItems);

            return View(pagedList);
        }

        [Route("Announcement/{id:int}/{referrerUsername?}")]
        public ViewResult Announcement(int id, string referrerUsername = null)
        {
            ViewBag.ReferrerUsername = referrerUsername;

            DTO.Announcement announcementDTO = this.announcementService.GetAnnouncementByID(id);
            ViewModels.PublicAnnouncement announcement = Mapper.Map<ViewModels.PublicAnnouncement>(announcementDTO);

            return View(announcement);
        }

        [Route("Opportunities/{referrerUsername?}")]
        public ViewResult Opportunities()
        {
            return View();
        }
        
        [Route("LocalPackages/{id:int}/{referrerUsername?}")]
        public ViewResult LocalPackages(int id, string referrerUsername = null)
        {
            ViewBag.ReferrerUsername = referrerUsername;
            ViewBag.SelectedID = id;
            return View();
        }

        [Route("ReferrerNotFound")]
        public ViewResult ReferrerNotFound()
        {
            return View();
        }
    }
}
