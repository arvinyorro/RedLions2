namespace RedLions.Application.DTO
{
    using System;
    using RedLions.Business;

    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisteredDateTime { get; set; }
    }

    internal static class UserAssembler
    {
        internal static DTO.User ToDTO(Business.User user)
        {
            return new DTO.User()
            {
                ID = user.ID,
                Username = user.Username,
                Password = user.Password
            };
        }
    }
}
