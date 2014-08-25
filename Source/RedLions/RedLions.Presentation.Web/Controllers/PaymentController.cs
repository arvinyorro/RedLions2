namespace RedLions.Presentation.Web.Controllers
{
    using System;
    using System.Linq;
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
        private MemberService memberService;

        public PaymentController(
            PaymentService paymentService,
            MemberService memberService)
        {
            this.paymentService = paymentService;
            this.memberService = memberService;
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
            var payment = new Models.Payment()
            {
                PaymentTypeID = typeID,
            };

            // If PayPal
            if (typeID == 2)
            {
                payment.PaymentMethod = "PayPal";
                payment.PayPalID = paypalID;
            }

            if (paypalID >= 25 && paypalID <= 46)
            {
                payment.Local = true;
            }
            else
            {
                payment.Local = false;
            }

            payment.AddReferrer(this.GetReferrer());
            ViewModels.CreatePayment createPaymentViewModel = this.GetCreatePaymentViewModel(payment);

            return View(createPaymentViewModel);
        }

        [HttpPost]
        public ActionResult Confirm(ViewModels.CreatePayment createPayment)
        {
            Models.Payment payment = createPayment.Payment;
            
            
            ICollection<DTO.PaymentGift> paymentGiftDtoList = this.paymentService
                .GetAllGifts()
                .ToList();

            var selectedPaymentGiftDtoList = new List<DTO.PaymentGift>();

            if (createPayment.Payment.Local)
            {
                foreach(DTO.PaymentGift paymentGiftDto in paymentGiftDtoList)
                {
                    bool selected = Request[paymentGiftDto.ID.ToString()].Split(',').FirstOrDefault() == "true" ? true : false;

                    if (selected)
                    {
                        selectedPaymentGiftDtoList.Add(paymentGiftDto);
                    }
                }

                decimal totalGiftPrice = selectedPaymentGiftDtoList.Select(x => x.Price).Sum();
                if (totalGiftPrice > 2500)
                {
                    ModelState.AddModelError("GiftCertificates", "Total gift certificate price must not exceed 2500");
                }
                else if (totalGiftPrice == 0)
                {
                    ModelState.AddModelError("GiftCertificates", "Please select at least 1 gift certificate");
                }
            }

            

            payment.BirthDate = this.ConvertToDate(
                month: createPayment.BirthMonth,
                day: createPayment.BirthDay,
                year: createPayment.BirthYear);

            if (payment.Age < 18)
            {
                ModelState.AddModelError("Payment.Age", "You must be 18 or above.");
            }

            // Success
            if (ModelState.IsValid)
            {
                DTO.Payment paymentDto = Mapper.Map<DTO.Payment>(payment);
                
                paymentDto.GiftCertificates = selectedPaymentGiftDtoList;

                this.paymentService.Create(paymentDto);

                string viewName = payment.PaymentTypeID == 1 ? "Confirm" : "PayPal";
                ViewBag.PayPalID = payment.PayPalID;               

                return View(viewName);
            }

            // Fail
            payment.AddReferrer(this.GetReferrer());
            ViewModels.CreatePayment createPaymentViewModel = this.GetCreatePaymentViewModel(payment);

            foreach (DTO.PaymentGift paymentGiftDto in selectedPaymentGiftDtoList)
            {
                var giftCertificateModel = createPaymentViewModel
                    .GiftCertificates
                    .FirstOrDefault(x => x.ID == paymentGiftDto.ID);

                if (giftCertificateModel != null)
                {
                    giftCertificateModel.Checked = true;
                }
            }

            return View("Register", createPaymentViewModel);
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

        private ViewModels.CreatePayment GetCreatePaymentViewModel(Models.Payment payment = null)
        {
            IEnumerable<DTO.PaymentGift> paymentGiftDtoList = this.paymentService.GetAllGifts();
            var viewModel = new ViewModels.CreatePayment()
            {
                Payment = payment,
                GiftCertificates = Mapper.Map<IEnumerable<Models.PaymentGift>>(paymentGiftDtoList),
                PaymentMethodList = this.GetPaymentMethodSelectList(payment.PaymentMethod),
                GenderList = this.GetGenderSelectList(payment.Gender),
                BirthDayList = this.GetBirthDayList(payment.BirthDate.Day.ToString()),
                BirthMonthList = this.GetBirthMonthList(payment.BirthDate.Month.ToString()),
                BirthYearList = this.GetBirthYearList(payment.BirthDate.Year.ToString()),
            };

            return viewModel;
        }
        

        private DTO.Member GetReferrer()
        {
            // Get Referrer
            HttpContext httpContext = base.HttpContext.ApplicationInstance.Context;
            HttpCookie cookie = httpContext.Request.Cookies.Get("ReferrerUsername");
            bool referrerNotInCookie = (httpContext.Request.Cookies["ReferrerUsername"] == null);
            if (referrerNotInCookie)
            {
                RedirectToAction("Index", "Home");
            }
            string referrerUsername = cookie.Value;
            return this.memberService.GetMemberByUsername(referrerUsername);
        }

        private SelectList GetBirthYearList(string selectedValue = null)
        {
            var years = new List<SelectListItem>();
            for (int x = DateTime.Now.Year; x >= 1920; x--)
            {
                years.Add(new SelectListItem()
                {
                    Value = x.ToString(),
                    Text = x.ToString(),
                });
            }

            return new SelectList(years, "Value", "Text", selectedValue);
        }

        private SelectList GetBirthMonthList(string selectedValue = null)
        {
            var months = new List<SelectListItem>();

            for (int x = 1; x <= 12; x++ )
            {
                months.Add(new SelectListItem()
                {
                    Value = x.ToString(),
                    Text = x.ToString(),
                });
            }

            return new SelectList(months, "Value", "Text", selectedValue);
        }

        private SelectList GetBirthDayList(string selectedValue = null)
        {
            var day = new List<SelectListItem>();

            for (int x = 1; x <= 31; x++)
            {
                day.Add(new SelectListItem()
                {
                    Value = x.ToString(),
                    Text = x.ToString(),
                });
            }

            return new SelectList(day, "Value", "Text", selectedValue);
        }

        private SelectList GetGenderSelectList(string selectedValue = null)
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

            return new SelectList(gender, "Value", "Text", selectedValue);
        }

        private SelectList GetPaymentMethodSelectList(string selectedValue = null)
        {
            var methods = new List<SelectListItem>();

            methods.Add(new SelectListItem() { Value = "Cebuana Lhuiller", Text = "Cebuana Lhuiller", });
            methods.Add(new SelectListItem() { Value = "ML Kwarta Padala", Text = "ML Kwarta Padala", });
            methods.Add(new SelectListItem() { Value = "Western Union", Text = "Western Union", });
            methods.Add(new SelectListItem() { Value = "Moneygram", Text = "Moneygram", });
            methods.Add(new SelectListItem() { Value = "LBC Express Remit", Text = "LBC Express Remit", });
            methods.Add(new SelectListItem() { Value = "JRS Pera Padala", Text = "JRS Pera Padala", });
            methods.Add(new SelectListItem() { Value = "Cebuana Lhuiller", Text = "Cebuana Lhuiller", });
            methods.Add(new SelectListItem() { Value = "Bank-to-Bank Deposit", Text = "Bank-to-Bank Deposit", });
            
            return new SelectList(methods, "Value", "Text", selectedValue);
        }

        private DateTime ConvertToDate(int month, int day, int year)
        {
            DateTime dateTime;
            DateTime.TryParse(String.Format("{0}-{1}-{2}", month, day, year), out dateTime);
            return dateTime;
        }

	}
}