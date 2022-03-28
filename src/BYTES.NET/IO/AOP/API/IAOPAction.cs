//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYTES.NET.IO.AOP.API
{
    public interface IAOPAction<TTarget>
    {
        #region public properties

        InterceptionTypes InterceptionType { get; }

        Action<string, object[], TTarget> Callback { get; }

        #endregion

        #region public method(s)

        bool MatchesMethod(string methodName);

        #endregion
    }
}
