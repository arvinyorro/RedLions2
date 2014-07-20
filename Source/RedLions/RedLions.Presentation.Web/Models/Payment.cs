namespace RedLions.Presentation.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DTO = RedLions.Application.DTO;

    public class Payment
    {
        public int ID { get; set; }

        [Required]
        public int PaymentTypeID { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [StringLength(1)]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        public int PayPalID { get; set; }

        public string PublicID { get; set; }

        public string ReferenceNumber { get; set; }

        public DateTime CreatedDateTime { get; set; }

    }
}