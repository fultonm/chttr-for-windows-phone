using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using wpchttr.Core;

namespace wpchttr.Test
{
    [TestClass]
    public class UserTest
    {
        CurrentUser currentUser;

        [TestInitialize]
        public void setup()
        {
            currentUser = new CurrentUser("fultonm@wartimestudios.com", "mike2009");
            currentUser.SignIn();
        }

        [TestMethod]
        public void CurrentUser_SignInValidCredentials_ReturnsTrue()
        {
            CurrentUser cu = new CurrentUser("fultonm@wartimestudios.com", "mike2009");
            Assert.IsTrue(cu.SignIn());
        }

        [TestMethod]
        public void CurrentUser_SignInInvalidCredentials_ReturnsFalse()
        {
            CurrentUser cu = new CurrentUser("fultonm@wartimestudios.com", "ike2009");
            Assert.IsTrue(cu.SignIn());
        }

        [TestMethod]
        public void CurrentUser_Relationships_ReturnsData()
        {
            Relationships relationships = currentUser.Relationships = new Relationships();
            Assert.IsTrue(relationships.Followers + relationships.Following > 0);
        }

        [TestMethod]
        public void CurrentUser_Chats_ReturnsChatCollection()
        {
            Chats chats = currentUser.Chats = new Chats(currentUser.Id);
            Assert.IsTrue(chats.ChatCollection.Count > 0);
        }

    }
}
