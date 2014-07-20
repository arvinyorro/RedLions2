namespace RedLions.Presentation.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using RedLions.Presentation.Web.Components;
    using RedLions.Presentation.Web.Models;
    using DTO = RedLions.Application.DTO;

    public class CreatePayment
    {
        public Payment Payment { get; set; }
        public int BirthDay { get; set; }
        public int BirthMonth { get; set; }
        public int BirthYear { get; set; }
        public SelectList PaymentMethodList { get; set; }
        public SelectList GenderList { get; set; }
        public SelectList BirthDayList { get; set; }
        public SelectList BirthMonthList { get; set; }
        public SelectList BirthYearList { get; set; }
    }
}