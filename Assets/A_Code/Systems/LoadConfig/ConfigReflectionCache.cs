using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace A_Code.Systems.LoadConfig
{
    public static class ConfigReflectionCache
    {
        public static IReadOnlyList<(Type type, string address)> AllConfigs { get; }

        static ConfigReflectionCache()
        {
            AllConfigs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return Array.Empty<Type>(); }
                })
                .Where(t => t.GetCustomAttribute<ConfigAddressAttribute>() != null)
                .Select(t =>
                {
                    var attr = t.GetCustomAttribute<ConfigAddressAttribute>();
                    return (t, attr.Address);
                })
                .ToList();
        }

    }
}