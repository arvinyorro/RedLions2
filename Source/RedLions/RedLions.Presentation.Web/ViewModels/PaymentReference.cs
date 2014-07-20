namespace RedLions.Presentation.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class PaymentReference
    {
        public int PaymentID { get; set; }

        [Required]
        public string ReferenceNumber { get; set; }
    }
}