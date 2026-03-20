//import .net (default) namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Cryptography;

namespace BYTES.NET.Tests.Cryptography
{
    [TestClass]
    public class TestAES
    {
        [TestMethod]
        public void TestSymmetricEncryptionDecryption()
        {
            string plainText = "Hello World!";

            //test using a 32 chars passphrase
            string password = "o28%BYTES.NET-2021!abfGhehw274hd";
            string encrypted = plainText.AES().Encrypt(password);

            Assert.AreEqual(plainText, encrypted.AES().Decrypt(password));

        }
    }
}
