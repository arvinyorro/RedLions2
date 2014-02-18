namespace RedLions.Application
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ServiceResult<T>
    {
        public StatusCode StatusCode { get; set; }
        public T Result { get; set; }
    }

    public enum StatusCode
    {
        Success,
        UserNotFound,
        UsernameInvalid,
        PasswordInvalid,
        DescriptionIsNull,
        ReferrerNotFound,
        DuplicateUsername,
        DuplicateEmail,
    }
}
