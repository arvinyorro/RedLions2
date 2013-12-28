namespace RedLions.Presentation.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using PagedList;
    using RedLions.Application;
    using DTO = RedLions.Application.DTO;

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

        public ActionResult Index()
        {
            int currentPage = this.GetCurrentPage();
            int totalInquiries = 0;
            IEnumerable<DTO.Inquiry> inquiryDTOList = this.inquiryService.GetPagedInquiries(currentPage, out totalInquiries);
            IEnumerable<Models.Inquiry> inquiryModels = inquiryDTOList.Select(x => new Models.Inquiry(x));

            int pageSize = this.inquiryService.PageSize;

            var pagedList = new StaticPagedList<Models.Inquiry>(inquiryModels, currentPage, pageSize, totalInquiries);
            return View(pagedList);
        }

        private int GetCurrentPage()
        {
            int page = 1;
            int.TryParse(HttpContext.Request.QueryString["page"], out page);
            return page == 0 ? 1 : page;
        }

    }
}
