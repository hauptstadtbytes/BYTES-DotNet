//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//import internal namespace(s) required
using BYTES.NET.IO.AOP.API;

namespace BYTES.NET.IO.AOP
{
    public class AOPPreInvokationAction<T> : IAOPAction<T>
    {
        #region private variable(s)

        private string _methodName = String.Empty;
        private Action<string, object[], T> _callback;

        #endregion

        #region public properties, implementing 'IAOPAction'

        public InterceptionTypes InterceptionType => InterceptionTypes.PreInvoke;

        public Action<string, object[], T> Callback => _callback;

        #endregion

        #region public new instance method(s)

        public AOPPreInvokationAction(string methodName, Action<string, object[], T> callback)
        {
            _methodName = methodName;
            _callback = callback;
        }

        #endregion

        #region public method(s) , implementing 'IAOPAction'

        public bool MatchesMethod(string methodName)
        {
            if (methodName.ToLower() == _methodName.ToLower())
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
