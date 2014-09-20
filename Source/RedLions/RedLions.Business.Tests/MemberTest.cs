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
               homeAddress: "#20 Road 1 Balubaran Valenzuela City Philippines",
               deliveryAddress: "#20 Road 1 Balubaran Valenzuela City Philippines",
               nationality: "Filipino",
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
            string expectedDeliveryAddress = "#20 Road 1 Balubaran Valenzuela City Philippines";
            string expectedHomeAddress = "#20 Road 1 Balubaran Valenzuela City Philippines";
            string expectedNationality = "Filipino";
            string expectedPassword = Encryption.Encrypt("redlions");
            DateTime expectedRegisteredDateTime = DateTime.Now;
            Role expectedUserRole = Role.Member;
            bool expectedDeactivated = false;
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
                homeAddress: expectedHomeAddress,
                deliveryAddress: expectedDeliveryAddress,
                nationality: expectedNationality,
                subscription: subscription,
                country: null);

            Assert.AreEqual(expectedUsername, member.Username, "Username should be {0}", expectedUsername);
            Assert.AreEqual(expectedFirstName, member.FirstName, "FirstName should be {0}", expectedFirstName);
            Assert.AreEqual(expectedLastName, member.LastName, "LastName should be {0}", expectedLastName);
            Assert.AreEqual(expectedEmail, member.Email, "Email should be {0}", expectedEmail);
            Assert.AreEqual(expectedPassword, member.Password, "Password should be {0}", expectedPassword);
            Assert.AreEqual(expectedUserRole, member.Role, "Role should be {0}", expectedUserRole);
            Assert.AreEqual(expectedRegisteredDateTime.Date, member.RegisteredDateTime.Date, "Registered Date should be {0}", expectedRegisteredDateTime.Date);
            Assert.AreEqual(expectedDeactivated, member.Deactivated, "Deactivated should be {0}", expectedDeactivated);
            Assert.AreEqual(expectedDeliveryAddress, member.DeliveryAddress, "Delivery address should be {0}", expectedDeliveryAddress);
            Assert.AreEqual(expectedHomeAddress, member.HomeAddress, "Home address should be {0}", expectedHomeAddress);
            Assert.AreEqual(expectedNationality, member.Nationality, "Nationality should be {0}", expectedNationality);
        }

        [TestMethod]
        public void SubscriptionShouldBeExpired()
        {
            // Setup
            Member member = this.CreateMember();

            // Setup expectactions.
            bool expectedResult = true;

            // Exercise
            // High jack date time.
            SystemTime.Now = new DateTime(2020, 1, 1);
            bool result = member.SubscriptionExpired;

            // Clean up.
            SystemTime.Now = DateTime.Now;

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
            expectedResult = expectedResult.AddMonths(subscription.Months);

            // Exercise
            member.ExtendSubscription(subscription);
            DateTime result = member.SubscriptionExpirationDateTime;

            // Verify
            Assert.AreEqual(expectedResult, result, "Result should be {0}", expectedResult);
        }

        [TestMethod]
        public void ShouldDeactivate()
        {
            // Setup
            Member member = this.CreateMember();
            bool expectedResult = true;

            // Exercise
            member.Deactivate();

            // Verify
            Assert.AreEqual(expectedResult, member.Deactivated, "Result should be {0}", expectedResult);
        }

        [TestMethod]
        public void ShouldActivate()
        {
            // Setup
            Member member = this.CreateMember();
            member.Deactivate();
            bool expectedDeactivatedStatus = false;

            // High jack date time. Fast forward into the future.
            SystemTime.Now = DateTime.Now.AddYears(1);

            TimeSpan remainingSubscription = member.SubscriptionExpirationDateTime.Subtract(member.DeactivatedDateTime.Value);
            DateTime expectedNewSubscriptionDate = SystemTime.Now.Add(remainingSubscription);

            // Exercise
            member.Activate();

            // Verify
            Assert.AreEqual(expectedDeactivatedStatus, member.Deactivated, "Result should be {0}", expectedDeactivatedStatus);
            Assert.AreEqual(
                expectedNewSubscriptionDate, 
                member.SubscriptionExpirationDateTime, 
                "Subscription expiration should be extended upon reactivation");

            // Clean up.
            SystemTime.Now = DateTime.Now;
        }
    }
}
