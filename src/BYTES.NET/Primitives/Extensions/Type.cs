//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives.Extensions
{
    public static class Type
    {
        #region public method(s)

        /// <summary>
        /// returns a boolean, indicating if an object has a specific property
        /// </summary>
        /// <param name="theType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasProperty(this System.Type theType, string name)
        {
            return theType.GetProperty(name) != null;
        }

        #endregion
    }
}
