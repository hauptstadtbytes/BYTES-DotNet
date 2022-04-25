using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.AOP.API
{
    public interface IAOPAction<T> where T : MarshalByRefObject
    {
        #region public properties

        Action<InterceptionType, string, T, object[]> ExecutionCallback { get; }

        #endregion

        #region public method(s)

        bool Matches(InterceptionType interception, string method, T data, object[] args);

        #endregion
    }
}
