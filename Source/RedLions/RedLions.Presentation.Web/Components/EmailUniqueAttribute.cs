namespace RedLions.Presentation.Web.Components
{
    // Built-in
    using System.ComponentModel.DataAnnotations;
    // Third Party
    using Microsoft.Practices.Unity;
    // Other Layers
    using RedLions.Application;

    public class EmailUniqueAttribute : ValidationAttribute
    {
        private UserService userService;

        public EmailUniqueAttribute(string errorMessage)
        {
            // Inject dependency
            this.userService = MvcApplication.Container.Resolve<UserService>();

            this.ErrorMessage = errorMessage;

            
        }

        public override bool IsValid(object value)
        {
            string email = value as string;
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }
            
            bool valid = (!this.userService.ExistsByEmail(email));

            return valid;
        }
    }
}