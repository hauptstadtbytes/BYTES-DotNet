//import .net namespace(s) required
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Primitives.Extensions;

namespace BYTES.NET.Test.Cryptography
{
    [TestClass]
    public class TestAES
    {
        [TestMethod]
        public void TestSymmetricEncryptionDecryption()
        {
            string source = "Hello World!";

            //test using a 32 chars passphrase
            string password = "o28%BYTES.NET-2021!abfGhehw274hd";
            string encrypted = source.AES().Encrypt(password);

            Assert.AreEqual(source, encrypted.AES().Decrypt(password));

        }
    }
}
