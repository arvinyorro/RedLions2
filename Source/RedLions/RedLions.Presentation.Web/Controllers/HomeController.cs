using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RedLions.Presentation.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            this.SaveReferralCodeToSession();
            return View();
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
