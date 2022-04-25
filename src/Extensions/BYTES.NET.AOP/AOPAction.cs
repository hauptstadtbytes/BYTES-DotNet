//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Primitives.Extensions;

//import internal namespace(s) required
using BYTES.NET.AOP.API;

namespace BYTES.NET.AOP
{
    public class AOPAction<T> : IAOPAction<T> where T : MarshalByRefObject
    {
        #region protected variable(s)

        protected InterceptionType _type;

        protected Func<InterceptionType, string, T, object[], bool> _evaluationCallback;
        protected Action<InterceptionType, string, T, object[]> _executionCallback;

        #endregion

        #region public properties, implementing 'IAOPAction<T>'

        public Action<InterceptionType, string, T, object[]> ExecutionCallback => _executionCallback;

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="evalCallback"></param>
        /// <param name="exeCallback"></param>
        /// <param name="interceptType"></param>
        public AOPAction(Func<InterceptionType, string, T, object[], bool> evalCallback, Action<InterceptionType, string, T, object[]> exeCallback, InterceptionType interceptType = InterceptionType.PreInvoke)
        {
            _evaluationCallback = evalCallback;
            _executionCallback = exeCallback;
            _type = interceptType;
        }

        /// <summary>
        /// overloaded new instance method, supporting the method name
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="exeCallback"></param>
        /// <param name="interceptType"></param>
        public AOPAction(string methodName, Action<InterceptionType, string, T, object[]> exeCallback, InterceptionType interceptType = InterceptionType.PreInvoke)
        {
            _evaluationCallback = (InterceptionType interception, string method, T data, object[] args) => {
                                                                                                                Regex myRegEx = new Regex("^" + methodName.Replace("*", "[\\w|\\W]*") + "$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                                                                                                                return method.MatchesPattern(myRegEx);
                                                                                                            };
            _executionCallback = exeCallback;
            _type = interceptType;
        }

        #endregion

        #region public method(s), implementing 'IAOPAction<T>'

        /// <summary>
        /// evaluates the current action for matching
        /// </summary>
        /// <param name="interception"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool Matches(InterceptionType interception, string method, T data, object[] args)
        {
            if (!interception.Equals(_type))
            {
                return false;
            }

            return _evaluationCallback(interception, method, data, args);
        }

        #endregion
    }
}
