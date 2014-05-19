namespace RedLions.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using RedLions.Business;
    using RedLions.CrossCutting;

    /// <summary>
    /// This class implements the <see cref="RedLions.Business.IUserRepository"/> interface.
    /// </summary>
    public class UserRepository : GenericRepository, IUserRepository
    {
        private IDbContext context;

        public UserRepository(IDbContext context)
            : base(context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
        }

        public void RegisterUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            base.Create(user);
            this.context.SaveChanges();
        }

        public void Update(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            this.context.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
            this.context.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return this.GetAll<User>();
        }

        public User GetUserByID(int userID)
        {
            return base.GetById<User>(userID);
        }

        public User GetUserByUsername(string username, string password)
        {
            return base.GetSingle<User>(x => x.Username == username && x.Password == password);
        }

        public User GetUserByUsername(string username)
        {
            return base.GetSingle<User>(x => x.Username == username);
        }

        public User GetUserByEmail(string email)
        {
            return base.GetSingle<User>(x => x.Email == email);
        }
    }
}
