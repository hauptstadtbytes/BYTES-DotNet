//import (default) .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.Primitives
{
    /// <summary>
    /// 'object' type extension method(s)
    /// </summary>
    public static class ObjecExtensions
    {
        #region public method(s)

        /// <summary>
        /// Checks a given object property for being null or default
        /// </summary>
        /// <param name="input"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks><seealso href="https://docs.microsoft.com/de-de/dotnet/csharp/language-reference/builtin-types/default-values"/></remarks>
        public static bool PropertyIsNullOrDefault(this object input, string name)
        {
            //get the property details
            PropertyInfo propInfo = input.GetType().GetProperty(name);
            Type propType = input.GetType().GetProperty(name).PropertyType;

            //compare the values
            if (propInfo.GetValue(input) == null)
            {
                return true;
            }

            if (propType == typeof(DateTime) && propInfo.GetValue(input).Equals(default(DateTime))) //for 'DateTime' type properties
            {
                return true;
            }
            else if (propType == typeof(bool) && propInfo.GetValue(input).Equals(default(bool))) //for 'boolean' type properties
            {
                return true;
            }
            else if (propType == typeof(int) && propInfo.GetValue(input).Equals(default(int))) //for 'integer' type properties
            {
                return true;
            }
            else if (propType == typeof(double) && propInfo.GetValue(input).Equals(default(double))) //for 'double' type properties
            {
                return true;
            }


            return false;
        }

        /// <summary>
        /// converts an object instance to another type
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputType"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// <remarks><seealso href="https://stackoverflow.com/questions/8625/generic-type-conversion-from-string"/></remarks>
        public static object? Convert(this object input, System.Type outputType, Func<object?, object>? callback = null)
        {
            System.Type underlyingType = Nullable.GetUnderlyingType(outputType);

            if (underlyingType != null && input == null)
            {
                return default;
            }
            else
            {
                System.Type basetype = underlyingType == null ? outputType : underlyingType;

                if (callback == null)
                {
                    return System.Convert.ChangeType(input, basetype);
                }
                else
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
        public static bool TryConvert<T>(this object? input, out T? output, Func<object?, T>? callback = null)
        {
            try
            {
                if (callback != null)
                {
                    output = (T)Convert(callback(input), typeof(T));
                }
                else
                {
                    output = (T)Convert(input, typeof(T));
                }

                return true;

            }
            catch (Exception ex)
            {
                output = default;
                return false;
            }
        }

        #endregion
    }
}
