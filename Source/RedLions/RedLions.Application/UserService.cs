namespace RedLions.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using RedLions.Application.DTO;
    using RedLions.Business;
    using RedLions.CrossCutting;

    public class UserService
    {
        private IRepository genericRepository;
        private IUserRepository userRepository;

        public UserService( 
            IRepository genericRepository,
            IUserRepository userRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException("The parameter 'userRepository' must not be null");
            }

            if (genericRepository == null)
            {
                throw new ArgumentNullException("The parameter 'genericRepository' must not be null");
            }

            this.userRepository = userRepository;
            this.genericRepository = genericRepository;
        }

        public DTO.User GetUserByID(int userID)
        {
            Business.User user = this.userRepository.GetUserByID(userID);

            if (user == null)
            {
                return null;
            }

            return UserAssembler.ToDTO(user);
        }

        public DTO.User GetUserByUsername(string username, string password)
        {
            Business.User user = this.userRepository.GetUserByUsername(username, password);

            if (user == null)
            {
                return null;
            }

            return UserAssembler.ToDTO(user);
        }

        public bool ExistsByUsername(string username)
        {
            int itemCount = this.genericRepository.Count<Business.User>(x => x.Username == username);
            return itemCount > 0 ? true : false;
        }

        public bool ExistsByEmail(string email)
        {
            int itemCount = this.genericRepository.Count<Business.User>(x => x.Email == email);
            return itemCount > 0 ? true : false;
        }

        public IEnumerable<DTO.Role> GetAllRoles()
        {
            return RoleAssembler.ToDTOList();
        }

        public IEnumerable<DTO.Role> GetUserRoles(string username)
        {
            Business.User user = this.userRepository.GetUserByUsername(username);
            var userRoles = new List<Business.Role>();
            userRoles.Add(user.Role);
            return RoleAssembler.ToDTOList(userRoles);
        }

        public StatusCode ChangePassword(int userID, string oldPassword, string newPassword)
        {
            Business.User user = this.userRepository.GetUserByID(userID);

            if (user == null)
            {
                throw new Exception("User ID not found");
            }

            if (user.Password != Encryption.Encrypt(oldPassword) ||
                string.IsNullOrEmpty(newPassword))
            {
                return StatusCode.PasswordInvalid;
            }

            user.ChangePassword(newPassword);
            this.userRepository.Update(user);

            return StatusCode.Success;
        }
    }
}
