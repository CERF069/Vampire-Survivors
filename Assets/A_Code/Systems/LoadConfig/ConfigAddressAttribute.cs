using System;

namespace A_Code.Systems.LoadConfig
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConfigAddressAttribute : Attribute
    {
        public string Address { get; }

        public ConfigAddressAttribute(string address)
        {
            Address = address;
        }
    }
}