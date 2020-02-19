using System;

namespace JsonKnownTypes.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public class JsonKnownTypeAttribute : Attribute
    {
        public Type Type { get; }
        public string Discriminator { get; }

        public JsonKnownTypeAttribute(Type type)
        {
            Type = type;
            Discriminator = type.Name;
        }

        public JsonKnownTypeAttribute(Type type, string discriminator)
        {
            Type = type;
            Discriminator = discriminator;
        }
    }
}
