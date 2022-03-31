//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives.Extensions
{
    public static class ObjectExtensions
    {
        #region public method(s)

        /// <summary>
        /// converts an object instance to another type
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputType"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// <remarks><seealso href="https://stackoverflow.com/questions/8625/generic-type-conversion-from-string"/></remarks>
        public static object? Convert(this object input, System.Type outputType, Func<object?,object>? callback = null)
        {
            System.Type underlyingType = Nullable.GetUnderlyingType(outputType);

            if (underlyingType != null && input == null)
            {
                return default;
            }
            else
            {
                System.Type basetype = underlyingType == null ? outputType : underlyingType;

                if(callback == null)
                {
                    return System.Convert.ChangeType(input, basetype);
                } else
                {
                    return System.Convert.ChangeType(callback(input), basetype);
                }
                
            }
        }

        /// <summary>
        /// trys to convert an object instance to a given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// <remarks><seealso href="https://stackoverflow.com/questions/8625/generic-type-conversion-from-string"/></remarks>
        public static bool TryConvert<T>(this object? input, out T? output, Func<object?,T>? callback = null)
        {
            try
            {
                if(callback != null)
                {
                    output = (T)Convert(callback(input), typeof(T));
                }
                else
                {
                    output = (T)Convert(input,typeof(T));
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
