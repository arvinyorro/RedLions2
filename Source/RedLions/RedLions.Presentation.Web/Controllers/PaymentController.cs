namespace RedLions.Presentation.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using RedLions.Presentation.Web.Components;
    using RedLions.Application;
    using DTO = RedLions.Application.DTO;
    using AutoMapper;

    public class PaymentController : Controller
    {
        private PaymentService paymentService;

        public PaymentController(
            PaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [Route("Payment/Start/{paypalID:int}")]
        public ViewResult Start(int paypalID)
        {
            var paymentTypeItems = new List<SelectListItem>();
            paymentTypeItems.Add(new SelectListItem()
            {
                Value = "1",
                Text = "Cash",
            });

            paymentTypeItems.Add(new SelectListItem()
            {
                Value = "2",
                Text = "Paypal",
            });

            var selectList = new SelectList(paymentTypeItems, "Value", "Text");

            ViewBag.PayPalID = paypalID;
            ViewBag.PaymentTypes = selectList;

            return View();
        }

        public ViewResult Register(int paypalID, int typeID)
        {
            ViewBag.GenderList = this.GetGenderSelectList();
            ViewBag.PaymentMethodList = this.GetPaymentMethodSelectList();

            var paymentModel = new Models.Payment()
            {
                PaymentTypeID = typeID,
            };

            // If PayPal
            if (typeID == 2)
            {
                paymentModel.PaymentMethod = "PayPal";
                paymentModel.PayPalID = paypalID;
            }

            return View(paymentModel);
        }

        [HttpPost]
        public ActionResult Confirm(Models.Payment payment)
        {
            if (ModelState.IsValid)
            {
                DTO.Payment paymentDto = Mapper.Map<DTO.Payment>(payment);
                this.paymentService.Create(paymentDto);

                string viewName = payment.PaymentTypeID == 1 ? "Confirm" : "PayPal";
                ViewBag.PayPalID = payment.PayPalID;

                return View(viewName);
            }

            ViewBag.GenderList = this.GetGenderSelectList();
            ViewBag.PaymentMethodList = this.GetPaymentMethodSelectList();
            return View("Register", payment);
        }

        [Route("Payment/Reference/{publicID}")]
        public ViewResult Reference(string publicID)
        {
            DTO.Payment paymentDto = this.paymentService.GetByPublicID(publicID);

            var paymentReferenceViewModel = new ViewModels.PaymentReference()
            {
                PaymentID = paymentDto.ID,
            };
            return View(paymentReferenceViewModel);
        }

        [HttpPost]
        public ViewResult ReferenceConfirm(ViewModels.PaymentReference paymentReference)
        {
            if (!ModelState.IsValid)
            {
                return View(paymentReference);
            }

            this.paymentService.ConfirmPayment(
                paymentReference.PaymentID, 
                paymentReference.ReferenceNumber);

            return View("ReferenceConfirm");
        }

        private SelectList GetGenderSelectList()
        {
            var gender = new List<SelectListItem>();
            gender.Add(new SelectListItem()
            {
                Value = "M",
                Text = "Male",
            });

            gender.Add(new SelectListItem()
            {
                Value = "F",
                Text = "Female",
            });

            return new SelectList(gender, "Value", "Text");
        }

        private SelectList GetPaymentMethodSelectList()
        {
            var method = new List<SelectListItem>();
            method.Add(new SelectListItem()
            {
                Value = "Cebuana",
                Text = "Cebuana",
            });

            return new SelectList(method, "Value", "Text");
        }
	}
}