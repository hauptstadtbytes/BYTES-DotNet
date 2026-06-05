using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BYTES.NET.IO;
using System.Net;

namespace BYTES.NET.Tests.IO
{
    [TestClass]
    public class TestUserInfo
    {
        UserInfo user;

        [TestInitialize]
        public void Setup()
        {
            user = new UserInfo("test", "admin", "test.domain");
        }


        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsNotNull(user.Name);
            Assert.IsNotNull(user.Password);
            Assert.IsNotNull(user.Domain);
            Assert.AreEqual(user.FullName, user.Name + "@" + user.Domain);
        }

        [TestMethod]
        public void TestToNetworkCredentials()
        {
            NetworkCredential cred = user.ToNetworkCredential();
            Assert.IsNotNull(cred.UserName);
            Assert.IsNotNull(cred.Password);
            Assert.IsNotNull(cred.Domain);

            Assert.AreEqual(cred.UserName, user.Name);
            Assert.AreEqual(cred.Password, user.Password);
            Assert.AreEqual(cred.Domain, user.Domain);
        }
    }
}
