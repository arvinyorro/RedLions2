namespace RedLions.Presentation.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using DTO = RedLions.Application.DTO;
    using RedLions.Application;
    using PagedList;
    using AutoMapper;

    public class PaymentsController : Controller
    {
        private PaymentService paymentService;

        public PaymentsController(
            PaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        public ViewResult Index(int? page, string searchEmail)
        {
            ViewBag.SearchEmail = searchEmail;

            int currentPage = (page ?? 1);

            // Fix negative page
            currentPage = currentPage < 0 ? 1 : currentPage;

            int pageSize = 20;
            int totalItems = 0;

            IEnumerable<DTO.Payment> paymentDtoList = this.paymentService
                .GetPagedList(
                    currentPage,
                    out totalItems,
                    pageSize,
                    searchEmail);

            int totalUnread = paymentDtoList.Count(x => x.AdminUnread == true);
            ViewBag.TotalUnread = totalUnread;

            IEnumerable<Models.Payment> announcementModels = Mapper.Map<IEnumerable<Models.Payment>>(paymentDtoList);

            var pagedList = new StaticPagedList<Models.Payment>(announcementModels, currentPage, pageSize, totalItems);

            return View(pagedList);
        }

        public ActionResult ReadAll()
        {
            this.paymentService.ReadAllPayments();
            return RedirectToAction("Index");
        }

        public ViewResult Details(int id)
        {
            DTO.Payment paymentDto = this.paymentService.GetByID(id);
            Models.Payment paymentModel = Mapper.Map<Models.Payment>(paymentDto);

            this.paymentService.ReadPayment(id);

            return View(paymentModel);
        }
	}
}