
namespace RedLions.Presentation.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DTO = RedLions.Application.DTO;
    using RedLions.Presentation.Web.Components;

    public class LoginForm
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public Role Role { get; set; }
    }

    public class ChangePassword
    {
        [Required(ErrorMessage="Please enter your old password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please enter your new password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "The {0} should be between {2} to {1} characters long.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage="Please type your new password again.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class User
    {
        [Required]
        [Display(Name = "Username")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The {0} should be between {2} to {1} characters long.")]
        [UsernameUnique("This username is already taken.")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(20, MinimumLength=5, ErrorMessage="The {0} should be between {2} to {1} characters long.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Retype Password")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "You must provide an email.")]
        [MaxLength(200)]
        [EmailUnique("This email is already taken.")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your first name.")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your last name.")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Display(Name = "Date Registered")]
        public DateTime RegisteredDateTime { get; set; }

        public Role Role { get; set; }
    }

    public class Role
    {
        public string Title { get; set; }
    }
}