using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using wpchttr.Core;

namespace wpchttr.Test
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void SignIn_ValidCredentials_ReturnsTrue()
        {
            CurrentUser cu = new CurrentUser("fultonm@wartimestudios.com", "mike2009");
            Assert.IsTrue(cu.SignIn());              
        }

        //[TestMethod]
        //public void SignIn_InvalidCredentials_ReturnsFalse()
        //{
        //    CurrentUser cu = new CurrentUser("fultonm@invalid", "mike2009");
        //    Assert.IsFalse(cu.SignIn());
        //}
    }
}
