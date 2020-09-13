using System;
using System.Reflection;
using UnityEngine;

namespace UnityUtils
{
    public static class CallMethodExtensions
    {
        private static CallOptions defaultOptions = new CallOptions();

        public static CallResponse<T> CallMethod<T>(this GameObject obj, string name, CallOptions options, params object[] args)
        {
            var response = new CallResponse<T>();
            Component[] components = options.AllowChildren ? obj.GetComponentsInChildren<Component>(false) : obj.GetComponents<Component>();
            foreach (var comp in components)
            {
                MethodInfo info = comp.GetType().GetMethod(name, options.flags);
                if (info != null && info.GetCustomAttribute<ExcludeFromCall>() == null)
                {
                    try
                    {
                        response.Add(comp, (T)info.Invoke(comp, args));
                    }
                    catch (InvalidCastException)
                    {
                        throw new TypeError("Invalid return type");
                    }
                    if (!options.AllowMultiple) break;
                }
            }
            return response;
        }

        public static CallResponse<T> CallMethod<T>(this GameObject obj, string name, params object[] args) => obj.CallMethod<T>(name, defaultOptions, args);

        public static CallResponse<object> CallMethod(this GameObject obj, string name, CallOptions options, params object[] args) => obj.CallMethod<object>(name, options, args);

        public static CallResponse<object> CallMethod(this GameObject obj, string name, params object[] args) => obj.CallMethod<object>(name, defaultOptions, args);
    }
}
