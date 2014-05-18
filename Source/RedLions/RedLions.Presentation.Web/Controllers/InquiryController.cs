using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RedLions.Presentation.Web.Models;

namespace RedLions.Presentation.Web.Controllers
{
    public class InquiryController : Controller
    {
        //
        // GET: /Inquire/
        public ActionResult Chat()
        {
            // Verify if there are any ongoing sessions.

            // If yes, go straight to chat mode.

            return View();
        }

        [HttpPost]
        public ActionResult Chat(InquiryChatRegister inquiryChatRegister)
        {
            if (!base.ModelState.IsValid)
            {
                return View();
            }

            var viewModel = new InquiryChatMessage() 
            { 
                InquiryChatSessionID = 1,
                Name = inquiryChatRegister.Name,
            };

            return View("ChatPage", viewModel);
        }
	}
}