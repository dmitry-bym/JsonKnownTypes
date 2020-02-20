using System;

namespace JsonKnownTypes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public class JsonKnownAttribute : Attribute
    {
        public Type Type { get; }
        public string Discriminator { get; }

        public JsonKnownAttribute(Type type)
        {
            Type = type;
            Discriminator = type.Name;
        }

        public JsonKnownAttribute(Type type, string discriminator)
        {
            Type = type;
            Discriminator = discriminator;
        }
    }
}
