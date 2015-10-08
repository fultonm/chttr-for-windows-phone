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
        public void Relationships_ReturnsRelationshipCounts()
        {
            Relationships relationships = currentUser.Relationships = new Relationships();
            Assert.IsTrue(relationships.FollowersCount + relationships.FollowingCount > 0);
        }

        [TestMethod]
        public void Chats_ReturnsCurrentUserChatCollection()
        {
            Chats chats = currentUser.Chats = new Chats(currentUser.Id);
            Assert.IsTrue(chats.ChatCollection.Count > 0);
        }

        [TestMethod]
        public void RequestFollowingChat_ReturnsNonEmptyFollowedChatContent()
        {
            Relationships relationships = currentUser.Relationships = new Relationships();
            User followed = currentUser.Relationships.Following[0];
            string chatContent = followed.Chats.ChatCollection[0].Content;
            Assert.IsFalse(string.IsNullOrEmpty(chatContent));
        }

        [TestMethod]
        public void SubmitChat_ChatCanBeRetrieved()
        {
            Chat chat = new Chat("Visual Studio unit testing chat submission.");
            Assert.IsTrue(chat.Submit());
            currentUser.Chats = new Chats(currentUser.Id);
            Chat retrievedChat = currentUser.Chats.ChatCollection[0];
            Assert.AreEqual(chat.Content, retrievedChat.Content);
        }
    }
}
