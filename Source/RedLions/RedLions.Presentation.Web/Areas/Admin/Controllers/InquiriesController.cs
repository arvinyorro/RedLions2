namespace RedLions.Presentation.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using DTO = RedLions.Application.DTO;
    using RedLions.Application;
    using PagedList;

    public class InquiriesController : Controller
    {
        private InquiryService inquiryService;

        public InquiriesController(InquiryService inquiryService)
        {
            if (inquiryService == null)
            {
                throw new ArgumentNullException("The inquiryService must not be null.");
            }

            this.inquiryService = inquiryService;
        }
        //
        // GET: /Admin/Inquiries/

        public ActionResult Index(int? page, string searchEmail)
        {
            ViewBag.SearchEmail = searchEmail;

            int currentPage = (page ?? 1);

            int totalInquiries = 0;

            IEnumerable<DTO.Inquiry> inquiryDTOList = this.inquiryService
                .GetPagedInquiries(
                    currentPage, 
                    out totalInquiries,
                    searchEmail);

            IEnumerable<Models.Inquiry> inquiryModels = inquiryDTOList.Select(x => new Models.Inquiry(x));

            int pageSize = this.inquiryService.PageSize;

            var pagedList = new StaticPagedList<Models.Inquiry>(inquiryModels, currentPage, pageSize, totalInquiries);
            return View(pagedList);
        }
    }
}
