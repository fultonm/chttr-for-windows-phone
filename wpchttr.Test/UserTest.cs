﻿using System;
using Windows.Devices.AllJoyn;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using wpchttr.Model;

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
    }
}
