using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BYTES.NET.IO.User;

namespace BYTES.NET.Tests.IO.Users
{
    [TestClass]
    public class TestInfo
    {
        [TestMethod]
        public void Constructor_SetsProperties_Correctly_NoDomainNoPassword()
        {
            var userInfo = new Info("testuser");

            Assert.AreEqual("testuser", userInfo.Name);
            Assert.AreEqual("testuser", userInfo.FullName);
            Assert.IsNull(userInfo.Domain);
            Assert.IsNull(userInfo.Password);
        }

        [TestMethod]
        public void Constructor_SetsProperties_Correctly_WithDomainAndPassword()
        {
            var userInfo = new Info("testuser", "secret", "MYDOMAIN");

            Assert.AreEqual("testuser", userInfo.Name);
            Assert.AreEqual("MYDOMAIN\\testuser", userInfo.FullName);
            Assert.AreEqual("MYDOMAIN", userInfo.Domain);
            Assert.AreEqual("secret", userInfo.Password);
        }

        [TestMethod]
        public void ToNetworkCredential_ReturnsCredential_WithoutDomain()
        {
            var userInfo = new Info("testuser", "password123");

            NetworkCredential cred = userInfo.ToNetworkCredential();

            Assert.AreEqual("testuser", cred.UserName);
            Assert.AreEqual("password123", cred.Password);
            Assert.AreEqual(string.Empty, cred.Domain);
        }

        [TestMethod]
        public void ToNetworkCredential_ReturnsCredential_WithDomain()
        {
            var userInfo = new Info("testuser", "password123", "MYDOMAIN");

            NetworkCredential cred = userInfo.ToNetworkCredential();

            Assert.AreEqual("testuser", cred.UserName);
            Assert.AreEqual("password123", cred.Password);
            Assert.AreEqual("MYDOMAIN", cred.Domain);
        }
    }
}
