

namespace RedLions.Infrastructure.Repository.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RedLions.CrossCutting;
    using RedLions.Business;
    using RedLions.Infrastructure.Repository;

    [TestClass]
    public class GenericRepositoryIntegration
    {
        private RedLionsContext context;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new RedLionsContext();
        }

        [TestMethod]
        public void ConnectionTest()
        {
            var users = this.context.Users.ToList();
            Assert.IsNotNull(users);
        }

        [TestMethod]
        public void GetSingleMemberTest()
        {
            Member user = this.context.Users.OfType<Member>().First();
            Assert.IsTrue(user is Member, "User type should be Member");
        }

        [TestMethod]
        public void GetSingleUserTest()
        {
            User user = this.context.Users.OfType<User>().First();
            Assert.IsTrue(user is User, "User type should be User");
        }

        [TestMethod]
        public void GetAllMembersTest()
        {
            IEnumerable<Member> user = this.context.Users.OfType<Member>().AsEnumerable();
            Assert.IsTrue(user.First() is Member, "User type should be Member");
        }
    }
}
