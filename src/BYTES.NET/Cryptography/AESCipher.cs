//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BYTES.NET.Cryptography
{
    /// <summary>
    /// a generic AES cipher class, intended for symmetric string encryption/ decryption
    /// </summary>
    /// <remarks> based on the article at 'https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/'</remarks>
    public class AESCipher
    {
        #region private variable(s)

        private string _val = null;

        #endregion

        #region public new instance method

        /// <summary>
        /// default new instance method(s)
        /// </summary>
        /// <param name="value"></param>
        public AESCipher(string value)
        {
            _val = value;
        }

        #endregion

        #region public method(s)

        /// <summary>
        /// generates a (valid) passphrase
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GeneratePassphrase(int size = 32)
        {
            //parse the argument(s)
            if (size != 32 & size != 16)
            {
                throw new ArgumentException("Passphrase has to be of size 16 or 32 characters");
            }

            //create a random key
            string[] source = GetPassphraseCharacters();
            string output = string.Empty;

            for (int i = 1; i <= size; i++)
            {
                Random rnd = new Random();
                int index = rnd.Next(0, source.Length - 1);
                output += source[index];
            }

            return output;
        }

        /// <summary>
        /// encrypts a string, using a passphrase given
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText, string password)
        {

            //parse the argument(s)
            if (password.Length != 32 & password.Length != 16)
            {
                throw new ArgumentException("Passphrase has to be of size 16 or 32 characters");
            }

            //encrypt the data
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(password);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);

        }

        /// <summary>
        /// encrypts the (locally cached) string, using the passphrase given
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Encrypt(string password)
        {
            return Encrypt(_val, password);
        }

        /// <summary>
        /// decrypts a string, using the passphrase given
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText, string password)
        {
            //parse the argument(s)
            if (password.Length != 32 & password.Length != 16)
            {
                throw new ArgumentException("Passphrase has to be of size 16 or 32 characters");
            }

            //decrypt the data
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(password);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// decrypts the (locally cached) string, using the passphrase given
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Decrypt(string password)
        {
            return Decrypt(_val, password);
        }

        #endregion

        #region private method(s)

        /// <summary>
        /// returns a list of all character strings suitable for password generation
        /// </summary>
        /// <returns></returns>
        private static string[] GetPassphraseCharacters()
        {

            List<string> output = new List<string>();

            for (int i = 65; i <= 90; i++) //upper case letters
            {
                output.Add(Convert.ToChar(i).ToString());
            }

            for (int i = 97; i <= 122; i++) //lower case letters
            {
                output.Add(Convert.ToChar(i).ToString());
            }

            foreach (int num in new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }) //numbers
            {
                output.Add(num.ToString());
            }

            foreach (string str in new string[] { "!", "-", "_", ".", "%", "/", "&" }) //special characters
            {
                output.Add(str);
            }

            return output.ToArray();

        }

        #endregion
    }
}
