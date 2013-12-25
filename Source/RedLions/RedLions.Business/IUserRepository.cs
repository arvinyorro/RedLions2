namespace RedLions.Business
{
    using System.Collections.Generic;
    public interface IUserRepository
    {
        void RegisterUser(User user);
        void Update(User user);
        IEnumerable<User> GetAllUsers();
        User GetUserByID(int userID);
        User GetUserByUsername(string username, string password);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
    }
}
