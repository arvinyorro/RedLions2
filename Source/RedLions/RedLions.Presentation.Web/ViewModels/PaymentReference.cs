namespace RedLions.Presentation.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using RedLions.Presentation.Web.Models;

    public class PaymentReference
    {
        public int PaymentID { get; set; }

        [Required]
        public string ReferenceNumber { get; set; }
    }
}