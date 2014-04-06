namespace RedLions.Presentation.Web.Controllers
{
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

        [Route("Company/{referrerUsername?}")]
        public ViewResult Company()
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

        [Route("Skin/{referrerUsername?}")]
        public ViewResult Skin()
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
    }
}
