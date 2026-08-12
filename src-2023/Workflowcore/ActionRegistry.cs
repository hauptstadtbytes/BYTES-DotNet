using System;
using System.Collections.Generic;
using System.Reflection;

namespace WorkflowcoreLib
{
    /// <summary>
    /// Translate the JSON methodnames to callable methods
    /// </summary>
    public class ActionRegistry
    {
        private readonly Dictionary<string, (object Instance, MethodInfo Method)> actions
            = new Dictionary<string, (object, MethodInfo)>(StringComparer.OrdinalIgnoreCase);

        public void Register(object instance)
        {
            Type type = instance.GetType();
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                actions[$"{type.Name}::{method.Name}"] = (instance, method);
            }
        }

        public (object Instance, MethodInfo Method) Resolve(string actionClass, string actionMethod)
        {
            string key = $"{actionClass}::{actionMethod}";
            if (actions.TryGetValue(key, out var entry))
            {
                return entry;
            }
            throw new InvalidOperationException($"Action '{key}' nicht registriert.");
        }
    }
}
