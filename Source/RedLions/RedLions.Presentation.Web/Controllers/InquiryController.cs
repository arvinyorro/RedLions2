using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedLions.Presentation.Web.Components;
using DTO = RedLions.Application.DTO;
using RedLions.Application;
using RedLions.Presentation.Web.Models;

namespace RedLions.Presentation.Web.Controllers
{
    [SaveReferrer]
    public class InquiryController : Controller
    {
        private InquiryChatService inquiryChatService;
        private MemberService memberService;

        public InquiryController(
            InquiryChatService inquiryChatService,
            MemberService memberService)
        {
            this.inquiryChatService = inquiryChatService;
            this.memberService = memberService;
        }

        [Route("Inquiry/Chat/{referrerUsername?}")]
        public ActionResult Chat()
        {
            // Verify if there are any ongoing sessions.
            HttpContext httpContext = base.HttpContext.ApplicationInstance.Context;
            HttpCookie cookie = httpContext.Request.Cookies.Get("ChatSession");
            bool ongoingChat = (httpContext.Request.Cookies["ChatSession"] != null);

            // If there is ongoing chat, go to chat mode.
            if (ongoingChat)
            {
                string chatSessionID = cookie.Values["ChatSessionID"];
                string inquirerName = cookie.Values["InquirerName"];

                
                var viewModel = new InquiryChatMessage()
                {
                    InquiryChatSessionID = int.Parse(chatSessionID),
                    Name = inquirerName,
                };

                cookie.Expires = DateTime.Now.AddDays(30);

                return View("ChatPage", viewModel);
            }
            
            return View();
        }

        [Route("Inquiry/Chat/{referrerUsername?}")]
        [HttpPost]
        public ActionResult Chat(InquiryChatRegister inquiryChatRegister)
        {
            if (!base.ModelState.IsValid)
            {
                return View();
            }
            
            HttpContext httpContext = base.HttpContext.ApplicationInstance.Context;

            // Retrieve referrer in cookie.
            HttpCookie cookie = httpContext.Request.Cookies.Get("ReferrerUsername");
            bool referrerNotInSession = (httpContext.Request.Cookies["ReferrerUsername"] == null);
            if (referrerNotInSession)
            {
                throw new Exception("Referrer not found.");
            }
            string referrerUsername = cookie.Value;

            // Create new session.
            DTO.Member memberDTO = this.memberService.GetMemberByUsername(referrerUsername);
            DTO.InquiryChatSession chatSesssionDTO = this.inquiryChatService
                .CreateSession(inquiryChatRegister.Name, memberDTO.MemberID);

            // Save new cookie.
            var newCookie = new HttpCookie("ChatSession");
            newCookie.Values["ChatSessionID"] = chatSesssionDTO.ID.ToString();
            newCookie.Values["InquirerName"] = chatSesssionDTO.InquirerName;
            newCookie.Expires = DateTime.Now.AddDays(30);
            httpContext.Response.Cookies.Set(newCookie);
                        
            // Create view model for page.
            var viewModel = new InquiryChatMessage() 
            {
                InquiryChatSessionID = chatSesssionDTO.ID,
                Name = inquiryChatRegister.Name,
            };
            return View("ChatPage", viewModel);
        }
	}
}