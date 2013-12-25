﻿namespace RedLions.Business.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RedLions.Business;

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
            string expectedPassword = "random";
            DateTime expectedRegisteredDateTime = DateTime.Now;
            Role expectedUserRole = Role.Admin;

            User user = new User(
                expectedUsername,
                expectedFirstName,
                expectedLastName,
                expectedEmail,
                expectedPassword);

            Assert.AreEqual(expectedUsername, user.Username, "Username should be {0}", expectedUsername);
            Assert.AreEqual(expectedFirstName, user.FirstName, "FirstName should be {0}", expectedFirstName);
            Assert.AreEqual(expectedLastName, user.LastName, "LastName should be {0}", expectedLastName);
            Assert.AreEqual(expectedEmail, user.Email, "Email should be {0}", expectedEmail);
            Assert.AreEqual(expectedPassword, user.Password, "Password should be {0}", expectedPassword);
            Assert.AreEqual(expectedUserRole, user.Role, "Role should be {0}", expectedUserRole);
            Assert.AreEqual(expectedRegisteredDateTime.Date, user.RegisteredDateTime.Date, "Registered Date should be {0}", expectedRegisteredDateTime.Date);
        }
    }
}
