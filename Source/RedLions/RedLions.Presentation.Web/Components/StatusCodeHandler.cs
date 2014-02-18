
namespace RedLions.Presentation.Web.Components
{
    using RedLions.Application;
    public class StatusCodeHandler
    {
        public static string ErrorCodeToString(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.ReferrerNotFound:
                    return "Referrer Username not found. Leave empty if you wish to set a random referrer.";
                case StatusCode.DuplicateUsername:
                    return "This username is already taken.";
                case StatusCode.UsernameInvalid:
                    return "This username must only contain letters, numbers, and underscores.";
                case StatusCode.DuplicateEmail:
                    return "This email is already taken.";
                case StatusCode.PasswordInvalid:
                    return "Password is invalid";
                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}