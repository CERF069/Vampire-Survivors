using System;
using System.Collections.Generic;

namespace A_Code.Systems.LoadConfig
{
    public sealed class ConfigStorage
    {
        private readonly Dictionary<Type, object> _configs = new();

        public int Count => _configs.Count;

        public void Set(Type type, object config)
        {
            _configs[type] = config;
        }

        public T Get<T>()
        {
            if (!_configs.TryGetValue(typeof(T), out var config))
                throw new Exception($"Config {typeof(T).Name} not loaded");

            return (T)config;
        }

        public bool Has<T>()
        {
            return _configs.ContainsKey(typeof(T));
        }
    }
}