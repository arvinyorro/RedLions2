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
        }
        //
        // GET: /Home/

        public ActionResult Index()
        {
            this.SaveReferralCodeToSession();
            return View();
        }

        public ViewResult Inquire()
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
                Email = inquiry.LastName,
                CellphoneNumber = inquiry.CellphoneNumber
            };

            if (Session["ReferralCode"] != null)
            {
                string referralCode = Session["ReferralCode"] as string;
                DTO.Member referrer = this.memberService.GetMemberByReferralCode(referralCode);
                if (referrer != null)
                {
                    inquiryDTO.ReferrerID = referrer.ID;
                }
            }

            this.inquiryService.SubmitInquiry(inquiryDTO);
            return RedirectToAction("Index", "Home");
        }

        public PartialViewResult NavigationBar()
        {
            return PartialView("_NavigationPartial");
        }

        private void SaveReferralCodeToSession()
        {
            string referralCode = Request.QueryString["r"];
            if (string.IsNullOrEmpty(referralCode))
            {
                return;
            }
            Session["ReferralCode"] = referralCode;
        }
    }
}
