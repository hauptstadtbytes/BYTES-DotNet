//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives
{
    /// <summary>
    /// 'Type' type extensin method(s)
    /// </summary>
    public static class TypeExtensions
    {
        #region public method(s)

        /// <summary>
        /// returns a boolean, indicating if an object has a specific property
        /// </summary>
        /// <param name="theType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool PropertyExists(this System.Type theType, string name)
        {
            return theType.GetProperty(name) != null;
        }

        #endregion
    }
}
