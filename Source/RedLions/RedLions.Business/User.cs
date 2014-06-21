namespace RedLions.Business
{
    using System;
    using System.Linq;
    using RedLions.CrossCutting;

    public class User
    {
        private const string DefaultPassword = "redlions";

        public User(
            string username,
            string firstName,
            string lastName,
            string email)
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

            this.Role = Role.Admin;
            this.RegisteredDateTime = DateTime.Now;
            this.ResetPassword();
        }

        protected User()
        {
            // Required by Entity Framework.
        }

        public int ID { get; private set; }
        public string Username { get; set; }
        public string Password { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public DateTime RegisteredDateTime { get; private set; }

        public void ChangePassword(string password)
        {
            this.Password = Encryption.Encrypt(password);
        }

        public void ResetPassword()
        {
            this.Password = Encryption.Encrypt(DefaultPassword);
        }
    }
}
