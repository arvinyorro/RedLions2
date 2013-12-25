namespace RedLions.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using RedLions.Business;

    /// <summary>
    /// This class implements the <see cref="RedLions.Business.IUserRepository"/> interface.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private RedLionsContext context;

        public UserRepository(RedLionsContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("The parameter 'context' must not be null");
            }

            this.context = context;
        }

        public void RegisterUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("The parameter 'user' must not be null");
            }

            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        public void Update(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("The parameter 'user' must not be null");
            }

            this.context.Entry<User>(user).State = System.Data.EntityState.Modified;
            this.context.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return this.context.Users.ToList();
        }

        public User GetUserByID(int userID)
        {
            return this.context.Users.FirstOrDefault(x => x.ID == userID);
        }

        public User GetUserByUsername(string username, string password)
        {
            return this.context.Users
                .FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public User GetUserByUsername(string username)
        {
            return this.context.Users
                .FirstOrDefault(x => x.Username == username);
        }

        public User GetUserByEmail(string email)
        {
            return this.context.Users
                .FirstOrDefault(x => x.Username == email);
        }
    }
}
