//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives.Extensions
{
    public static class Object
    {
        #region public method(s)

        public static bool TryConvert<T>(this object? input, out T? output, Func<object,T>? callback = null)
        {
            try
            {
                if(callback != null)
                {
                    output = callback(input);
                } else
                {
                    Type underlyingType = Nullable.GetUnderlyingType(typeof(T));

                    if (underlyingType != null && input == null)
                    {
                        output = default;
                    } else
                    {
                        Type basetype = underlyingType == null ? typeof(T) : underlyingType;
                        output = (T)Convert.ChangeType(input, basetype);
                    }
                }

                return true;
                
            }catch(Exception ex)
            {
                output = default;
                return false;
            }
        }

        #endregion
    }
}
