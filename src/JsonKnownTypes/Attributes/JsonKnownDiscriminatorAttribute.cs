using System;

namespace JsonKnownTypes.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class JsonKnownDiscriminatorAttribute : Attribute
    {
        public string Name { get; set; } = "$type";
        public bool AutoJsonKnownType { get; set; } = true;
    }
}
