namespace RedLions.Business.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RedLions.Business;
    using RedLions.CrossCutting;
    using FakeItEasy;

    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void CreateUserTest()
        {
            string expectedUsername = "testUsername";
            string expectedFirstName = "firstname";
            string expectedLastName = "lastname";
            string expectedEmail = "yorro.a@gmail.com";
            string expectedPassword = Encryption.Encrypt("redlions");
            DateTime expectedRegisteredDateTime = DateTime.Now;
            Role expectedUserRole = Role.Admin;

            User user = new User(
                expectedUsername,
                expectedFirstName,
                expectedLastName,
                expectedEmail);

            Assert.AreEqual(expectedUsername, user.Username, "Username should be {0}", expectedUsername);
            Assert.AreEqual(expectedFirstName, user.FirstName, "FirstName should be {0}", expectedFirstName);
            Assert.AreEqual(expectedLastName, user.LastName, "LastName should be {0}", expectedLastName);
            Assert.AreEqual(expectedEmail, user.Email, "Email should be {0}", expectedEmail);
            Assert.AreEqual(expectedPassword, user.Password, "Password should be {0}", expectedPassword);
            Assert.AreEqual(expectedUserRole, user.Role, "Role should be {0}", expectedUserRole);
            Assert.AreEqual(expectedRegisteredDateTime.Date, user.RegisteredDateTime.Date, "Registered Date should be {0}", expectedRegisteredDateTime.Date);
        }

        [TestMethod]
        public void ShouldChangePassword()
        {
            // Setup
            string expectedPassword = Encryption.Encrypt("other");
            User user = A.Fake<User>();

            // Exercise
            user.ChangePassword("other");

            // Verify
            Assert.AreEqual(expectedPassword, user.Password, "Password should be {0}", expectedPassword);
        }

        [TestMethod]
        public void ShouldResetPassword()
        {
            // Setup
            string expectedPassword = Encryption.Encrypt("redlions");
            User user = A.Fake<User>();
            user.ChangePassword("other");

            // Exercise
            user.ResetPassword();

            // Verify
            Assert.AreEqual(expectedPassword, user.Password, "Password should be {0}", expectedPassword);
        }
    }
}
