using System;
using System.Collections.Generic;
using System.Reflection;

namespace AspiringDemo.Saving
{
    public class RuntimeTypeHandler
    {
        public Dictionary<Type, RuntimeType> Runtimes { get; set; }
        private Assembly _assembly;

        public RuntimeTypeHandler(Assembly assembly)
        {
            Runtimes = new Dictionary<Type, RuntimeType>();
            _assembly = assembly;
        }

        //TODO: Include private properties
        public void GetRuntimeInfo(Type type)
        {
            // add self if not exists
            if (!Runtimes.ContainsKey(type))
            {
                Runtimes.Add(type, new RuntimeType(type, type.Name));
                GetRuntimeInfo(type);
            }

            //
            foreach (var subprop in type.GetProperties())
            {
                var ass = Assembly.GetAssembly(subprop.PropertyType);

                if (Assembly.GetAssembly(subprop.PropertyType) != _assembly)
                {
                    CheckGenerics(subprop.PropertyType);
                    continue;
                }

                if (!Runtimes.ContainsKey(subprop.PropertyType))
                {
                    Runtimes.Add(subprop.PropertyType, new RuntimeType(subprop.PropertyType, subprop.Name));
                    GetRuntimeInfo(subprop.PropertyType);
                }
            }
        }

        private void CheckGenerics(Type type)
        {

            if (type.IsGenericType && type.GetGenericTypeDefinition()
                == typeof(List<>))
            {
                Type itemType = type.GetGenericArguments()[0]; // use this...
                var ass = Assembly.GetAssembly(itemType);
                if (!Runtimes.ContainsKey(itemType) && ass == _assembly)
                {
                    Runtimes.Add(itemType, new RuntimeType(itemType, itemType.Name));
                    GetRuntimeInfo(itemType);
                }
            }
        }
    }
}
