//import .net namespace required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

#if NETFULL
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
#endif

//import internal namespace(s) required
using BYTES.NET.IO.AOP.API;

namespace BYTES.NET.IO.AOP
{
    public enum InterceptionTypes
    {
        PreInvoke = 0,
        PostInvoke = 1
    }

#if NETFULL

    public class AOPProxy<T> : RealProxy where T : MarshalByRefObject
    {
        #region private variable(s)

        private T _target;

        private Dictionary<InterceptionTypes, List<IAOPAction<T>>> _actions = new Dictionary<InterceptionTypes, List<IAOPAction<T>>>();

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="target"></param>
        public AOPProxy(T target, IAOPAction<T>[]? actions = null) : base(typeof(T))
        {
            _target = target;
            
            if(actions != null)
            {
                foreach (IAOPAction<T> action in actions)
                {
                    if (!_actions.ContainsKey(action.InterceptionType))
                    {
                        _actions.Add(action.InterceptionType, new List<IAOPAction<T>>());
                    }

                    _actions[action.InterceptionType].Add(action);
                }
            }
        }

        #endregion

        #region public static method(s)

        /// <summary>
        /// create a new proxy instance
        /// </summary>
        /// <param name="target"></param>
        /// <param name="actions"></param>
        public static T Create(T target, IAOPAction<T>[] actions = null)
        {
            return new AOPProxy<T>(target, actions).GetTransparentProxy() as T;
        }

        #endregion

        #region public method(s), inherited from 'RealProxy' type

        public override IMessage Invoke(IMessage msg)
        {
            if (msg is IMethodCallMessage methodCallMessage)
            {
                try
                {
                    //parse the properties
                    var methodArgs = methodCallMessage.Args;
                    string methodName = methodCallMessage.MethodName;

                    AOPProxy<T> me = this;

                    //execute a pre-invoking action(s)
                    foreach (IAOPAction<T> action in GetMatchingActions(InterceptionTypes.PreInvoke, methodName))
                    {
                        action.Callback(methodName, methodArgs, _target);
                    }

                    //invoke the native/ base method
                    var result = methodCallMessage.MethodBase.Invoke(_target, methodArgs);

                    //execute a post-invoking action(s)
                    foreach (IAOPAction<T> action in GetMatchingActions(InterceptionTypes.PostInvoke, methodName))
                    {
                        action.Callback(methodName, methodArgs, _target);
                    }

                    //return the output
                    return new ReturnMessage(result, methodArgs, methodArgs.Length, methodCallMessage.LogicalCallContext, methodCallMessage);

                }
                catch (TargetInvocationException ex)
                {
                    return new ReturnMessage(ex.InnerException, methodCallMessage);
                }
            }
            else
            {
                throw new ArgumentException("IMethodCallMessage' expected", nameof(msg));
            }
        }

        #endregion

        #region private method(s)

        /// <summary>
        /// validates the actions against the current method call and returns a compatible list
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private IAOPAction<T>[] GetMatchingActions(InterceptionTypes type, string methodName)
        {
            if (!_actions.ContainsKey(type)) //there is no action registrated
            {
                return new IAOPAction<T>[] { };

            }

            //create a list of all registrated actions
            List<IAOPAction<T>> output = new List<IAOPAction<T>>();

            foreach (IAOPAction<T> action in _actions[type])
            {
                if (action.MatchesMethod(methodName))
                {
                    output.Add(action);
                }
            }

            return output.ToArray();
        }

        #endregion
    }

#endif

}
