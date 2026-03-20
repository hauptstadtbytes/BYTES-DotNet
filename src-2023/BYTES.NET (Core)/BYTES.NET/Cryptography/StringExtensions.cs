using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Cryptography
{
    /// <summary>
    /// string type extension method(s)
    /// </summary>
    public static class StringExtensions
    {
        #region public method(s)

        /// <summary>
        /// returns an instance of 'AESCipher' for a tsring given
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static AESCipher AES(this string text)
        {
            return new AESCipher(text);
        }

        #endregion
    }
}
