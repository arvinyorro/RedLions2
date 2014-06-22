namespace RedLions.Business.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RedLions.Business;
    using RedLions.CrossCutting;
    using FakeItEasy;

    [TestClass]
    public class MemberTest
    {
        public Member CreateMember()
        {
            var subscription = new Subscription("Silver", 3);
            Member member = new Member(
               username: "username",
               unoID: "1234",
               firstName: "Arvin",
               lastName: "Yorro",
               email: "yorro.a@gmail.com",
               personalReferralCode: "E0G9YK1KZ3RYBSYB2XXX",
               cellphoneNumber: "09276865083",
               subscription: subscription,
               country: null);

            return member;
        }

        [TestMethod]
        public void ShouldCreateMember()
        {
            // Setup
            string expectedUsername = "testUsername";
            string expectedUnoId = "12345";
            string expectedFirstName = "firstname";
            string expectedLastName = "lastname";
            string expectedEmail = "yorro.a@gmail.com";
            string expectedPassword = Encryption.Encrypt("redlions");
            DateTime expectedRegisteredDateTime = DateTime.Now;
            Role expectedUserRole = Role.Member;
            var subscription = new Subscription("Silver", 3);
            
            // Exercise
            Member member = new Member(
                username: expectedUsername,
                unoID: expectedUnoId,
                firstName: expectedFirstName,
                lastName: expectedLastName,
                email: expectedEmail,
                personalReferralCode: "E0G9YK1KZ3RYBSYB2XXX", 
                cellphoneNumber: "09276865083",
                subscription: subscription,
                country: null);

            Assert.AreEqual(expectedUsername, member.Username, "Username should be {0}", expectedUsername);
            Assert.AreEqual(expectedFirstName, member.FirstName, "FirstName should be {0}", expectedFirstName);
            Assert.AreEqual(expectedLastName, member.LastName, "LastName should be {0}", expectedLastName);
            Assert.AreEqual(expectedEmail, member.Email, "Email should be {0}", expectedEmail);
            Assert.AreEqual(expectedPassword, member.Password, "Password should be {0}", expectedPassword);
            Assert.AreEqual(expectedUserRole, member.Role, "Role should be {0}", expectedUserRole);
            Assert.AreEqual(expectedRegisteredDateTime.Date, member.RegisteredDateTime.Date, "Registered Date should be {0}", expectedRegisteredDateTime.Date);
        }

        [TestMethod]
        public void SubscriptionShouldBeExpired()
        {
            // Setup
            // High jack date time.
            SystemTime.Now = new DateTime(2020, 1, 1);
            Member member = this.CreateMember();

            // Setup expectactions.
            bool expectedResult = true;

            // Exercise
            bool result = member.SubscriptionExpired;

            // Verify
            Assert.AreEqual(expectedResult, result, "Result should be {0}", expectedResult);
        }

        [TestMethod]
        public void SubscriptionShouldNotBeExpired()
        {
            // Setup
            Member member = this.CreateMember();

            // Setup expectactions.
            bool expectedResult = false;

            // Exercise
            bool result = member.SubscriptionExpired;

            // Verify
            Assert.AreEqual(expectedResult, result, "Result should be {0}", expectedResult);
        }

        [TestMethod]
        public void ShouldExtendSubscription()
        {
            // Setup
            Member member = this.CreateMember();
            var subscription = new Subscription("Silver", 3);

            // Setup expectactions.
            DateTime expectedResult = member.SubscriptionExpirationDateTime;
            expectedResult.AddMonths(subscription.Months);

            // Exercise
            member.ExtendSubscription(subscription);
            DateTime result = member.SubscriptionExpirationDateTime;

            // Verify
            Assert.AreEqual(expectedResult, result, "Result should be {0}", expectedResult);
        }
    }
}
