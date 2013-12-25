namespace RedLions.Presentation.Web.Security
{
    // Built-in
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    // Third Party
    using Microsoft.Practices.Unity;
    // Outer Layer
    using RedLions.Application;
    using DTO = RedLions.Application.DTO;
    using RedLions.Infrastructure.Repository;

    public class CustomRoleProvider : RoleProvider
    {
        public UserService UserService { get; set; }

        public CustomRoleProvider()
            : base()
        {
            this.UserService = UnityConfig.GetConfiguredContainer().Resolve<UserService>();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            Models.LoginForm userModel = (Models.LoginForm)HttpContext.Current.User;

            if (userModel != null)
                return userModel.Role.Title == roleName;
            else
                return false;
        }

        public override string[] GetAllRoles()
        {
            IEnumerable<DTO.Role> roleDTOs = this.UserService.GetAllRoles();
            return roleDTOs.Select(x => x.Title).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            IEnumerable<DTO.Role> roleDTOs = this.UserService.GetUserRoles(username);
            return roleDTOs.Select(x => x.Title).ToArray();
        }

        #region Unimplemented MembershipProvider Methods
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }
        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }
        
        #endregion
    }
}