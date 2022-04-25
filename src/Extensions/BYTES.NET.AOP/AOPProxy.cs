//import .net namespace(s) required
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NETFULL
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
#endif

//import internal namespace(s) required
using BYTES.NET.AOP.API;

namespace BYTES.NET.AOP
{
    public enum InterceptionType
    {
        PreInvoke = 0,
        PostInvoke = 1
    }

#if NETFULL

    public class AOPProxy<T> : RealProxy where T : MarshalByRefObject
    {
        #region private variable(s)

        private T _target;
        private IAOPAction<T>[] _actions = new IAOPAction<T>[] { };

        #endregion

        #region public new instance method(s)

        /// <summary>
        /// default new instance method
        /// </summary>
        /// <param name="target"></param>
        /// <param name="actions"></param>
        public AOPProxy(T target, IAOPAction<T>[]? actions = null) : base(typeof(T))
        {
            _target = target;
                
            if(actions != null)
            {
                _actions = actions;
            }
                
         }

        #endregion

        #region public static method(s)

        /// <summary>
        /// create a new proxy instance
        /// </summary>
        /// <param name="target"></param>
        /// <param name="actions"></param>
        public static T Create(T target, IAOPAction<T>[]? actions = null)
        {
            return new AOPProxy<T>(target,actions).GetTransparentProxy() as T;
        }

         #endregion

         #region public method(s), inherited from 'RealProxy' type

         public override IMessage Invoke(IMessage msg)
         {
            if (msg is IMethodCallMessage methodCallMessage)
            {
                //parse the properties
                var args = methodCallMessage.Args;
                string name = methodCallMessage.MethodName;

                //Console.WriteLine("method " + name + " requested");

                AOPProxy<T> me = this;

                //execute the pre-invoking action(s)
                foreach(IAOPAction<T> action in _actions)
                {
                    if (action.Matches(InterceptionType.PreInvoke, name, _target, args))
                    {
                        action.ExecutionCallback(InterceptionType.PreInvoke, name, _target, args);
                    }
                }

                //invoke the native/ base method
                object result = methodCallMessage.MethodBase.Invoke(_target, methodCallMessage.Args);

                //execute the post-invoking action(s)
                foreach (IAOPAction<T> action in _actions)
                {
                    if (action.Matches(InterceptionType.PostInvoke, name, _target, args))
                    {
                        action.ExecutionCallback(InterceptionType.PreInvoke, name, _target, args);
                    }
                }

                //return the output value
                return new ReturnMessage(result, methodCallMessage.Args, methodCallMessage.Args.Length, methodCallMessage.LogicalCallContext, methodCallMessage);

            } else
            {
                throw new ArgumentException("IMethodCallMessage' expected", nameof(msg));
            }

         }

         #endregion
        }

#endif
}
