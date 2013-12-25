﻿namespace RedLions.Presentation.Web.Controllers
{
    // Built-in
    using System.Web.Mvc;
    using System.Web.Security;
    // Third Party
    using Microsoft.Practices.Unity;
    // Other Layers
    using RedLions.Application;
    // Internals
    using RedLions.Presentation.Web.Security;
    using DTO = RedLions.Application.DTO;

    [Authorize]
    public class AccountController : Controller
    {
        [Dependency]
        public MemberService MemberService { get; set; }

        public CustomMembershipProvider provider = (CustomMembershipProvider)Membership.Provider;

        [AllowAnonymous]
        public PartialViewResult LoginForm(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("LoginFormPartial");
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.LoginForm loginForm, string returnUrl)
        {
            if (ModelState.IsValid &&
                this.provider.ValidateUser(loginForm.Username, loginForm.Password))
            {
                FormsAuthentication.SetAuthCookie(loginForm.Username, loginForm.RememberMe);
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(loginForm);
        }

        [AllowAnonymous]
        public ViewResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(Models.User user)
        {
            if (string.IsNullOrEmpty(user.Password) ||
                string.IsNullOrWhiteSpace(user.Password))
            {
                ModelState.AddModelError("Password", "The password field is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var memberDTO = new DTO.Member()
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Email = user.Email,
            };

            if (Session["ReferralCode"] != null)
            {
                string referralCode = Session["ReferralCode"] as string;
                DTO.Member referrer = this.MemberService.GetMemberByReferralCode(referralCode);
                if (referrer != null)
                {
                    memberDTO.ReferrerUsername = referrer.Username;
                }
            }
            
            this.MemberService.Register(memberDTO);
            FormsAuthentication.SetAuthCookie(user.Username, false);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginPartial(Models.LoginForm loginForm, string returnUrl)
        {
            if (ModelState.IsValid &&
                this.provider.ValidateUser(loginForm.Username, loginForm.Password))
            {
                FormsAuthentication.SetAuthCookie(loginForm.Username, loginForm.RememberMe);
                return Json(new { Success = true } );
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return PartialView("LoginFormPartial", loginForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
