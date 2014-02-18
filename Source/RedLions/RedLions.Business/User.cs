namespace RedLions.Business
{
    using System;
    using System.Linq;

    public class User
    {
        public User(
            string username,
            string firstName,
            string lastName,
            string email,
            string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("Username must not be null or empty.");
            }

            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException("First name must not be null or empty.");
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException("Last name must not be null or empty.");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("Email must not be null or empty.");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Password must not be null or empty.");
            }

            // Allow only alphahumeric characters and underscore.
            bool usernameInvalid = !username.All(c => Char.IsLetterOrDigit(c) || c == '_');
            if (usernameInvalid)
            {
                throw new Exception("Username must only contain alphanumeric or underscore characters.");
            }

            this.Username = username;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Password = password;

            this.Role = Role.Admin;

            this.RegisteredDateTime = DateTime.Now;
        }

        protected User()
        {
            // Required by Entity Framework.
        }

        public int ID { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public DateTime RegisteredDateTime { get; private set; }

        public void ChangePassword(string password)
        {
            this.Password = password;
        }
    }
}
