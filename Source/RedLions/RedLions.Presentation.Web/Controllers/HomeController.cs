namespace RedLions.Presentation.Web.Controllers
{
    using System.Web.Mvc;
    using RedLions.Application;
    using DTO = RedLions.Application.DTO;


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

        [Route("{referrerUsername?}")]
        public ActionResult Index(string referrerUsername)
        {
            this.SaveReferrerCodeToSession(referrerUsername);
            return View();
        }

        public ViewResult Inquire()
        {
            return View();
        }

        [Route("Products/{referrerUsername?}")]
        public ViewResult Products(string referrerUsername)
        {
            this.SaveReferrerCodeToSession(referrerUsername);
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

        public ViewResult Awards()
        {
            return View();
        }

        public ViewResult Beverages()
        {
            return View();
        }

        public ViewResult Nutritions()
        {
            return View();
        }

        public ViewResult Personal()
        {
            return View();
        }

        public ViewResult Skin()
        {
            return View();
        }

        public ViewResult LocalBusiness()
        {
            return View();
        }

        public ViewResult LocalPackages()
        {
            return View();
        }

        private void SaveReferrerCodeToSession(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return;
            }

            Session["ReferrerUsername"] = username;

            // We need to remove the referrerUsername in the URL, otherwise
            // the MVC framework will generate this parameter all over our
            // links.
            Request.RequestContext.RouteData.Values.Remove("referrerUsername");
        }
    }
}
