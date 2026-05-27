using BYTES.NET.Windows.IO.LDAP;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Windows.Tests.IO
{
    [TestClass]
    public class TestLDAPManager
    {
        private LDAPManager _manager;

        [TestInitialize]
        public void Setup()
        {
            _manager = new LDAPManager("ldap://localhost:389");
        }

        [TestMethod]
        public void TestConstructor()
        {
            Debug.WriteLine(_manager);
            Assert.IsNotNull(_manager);
        }

        [TestMethod]
        public void TestGetCurrentDomain()
        {
            var domain = LDAPManager.GetCurrentDomain();
            Debug.WriteLine(domain);
            Assert.IsNotNull(domain);
        }

        [TestMethod]
        public void TestGetCurrentDomainWithPrefix()
        {
            string path = LDAPManager.GetCurrentDomain(true);

            Assert.IsFalse(string.IsNullOrEmpty(path));

            Assert.IsTrue(path.StartsWith("LDAP://"));
        }

        [TestMethod]
        public void TestAuthenticateInvalidUser()
        {
            bool result = _manager.Authenticate(
                "invalid_user",
                "invalid_password");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestSearch()
        {
            var results = _manager.Search(
                "(objectClass=user)",
                new[] { "cn" });

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void TestGetProperties()
        {
            var properties = _manager.GetProperties(
                "(objectClass=user)");

            Assert.IsNotNull(properties);

            Assert.IsTrue(properties.Length > 0);
        }
    }
}