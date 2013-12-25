namespace RedLions.Presentation.Web.Components
{
    // Built-in
    using System.ComponentModel.DataAnnotations;
    // Third Party
    using Microsoft.Practices.Unity;
    // Other Layers
    using RedLions.Application;

    public class UsernameUniqueAttribute : ValidationAttribute
    {
        private UserService userService;
        public UsernameUniqueAttribute(string errorMessage)
        {
            // Inject dependency
            this.userService = MvcApplication.Container.Resolve<UserService>();
            this.ErrorMessage = errorMessage;
        }

        public override bool IsValid(object value)
        {
        string username = value as string;
        if (string.IsNullOrEmpty(username))
        {
            return true;
        }

            bool valid = (!this.userService.ExistsByUsername(username));

            return valid;
        }
    }
}