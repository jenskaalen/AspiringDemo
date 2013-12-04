using System;
using System.Collections.Generic;

namespace AspiringDemo.Saving
{
    public class RuntimeTypeHandler
    {
        public Dictionary<string, RuntimeType> Runtimes { get; set; }

        public RuntimeTypeHandler()
        {
            Runtimes = new Dictionary<string, RuntimeType>();
        }

        public void GetRuntimeInfo(Type type)
        {
            foreach (var subprop in type.GetProperties())
            {
                if (IsNativeType(subprop.PropertyType))
                    continue;

                if (!Runtimes.ContainsKey(subprop.PropertyType.FullName))
                {
                    Runtimes.Add(subprop.PropertyType.FullName, new RuntimeType(subprop.PropertyType));
                    //CheckProperty(subprop.GetType());
                    GetRuntimeInfo(subprop.PropertyType);
                }
            }
        }

        private bool IsNativeType(Type type)
        {
            if (type == typeof(string))
                return true;
            if (type == typeof(double))
                return true;
            if (type == typeof(int))
                return true;
            if (type == typeof(long))
                return true;
            if (type == typeof(float))
                return true;
            if (type == typeof(bool))
                return true;

            return false;
        }
    }
}
