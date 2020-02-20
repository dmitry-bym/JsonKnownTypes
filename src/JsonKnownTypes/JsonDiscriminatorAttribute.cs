using System;

namespace JsonKnownTypes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class JsonDiscriminatorAttribute : Attribute
    {
        public string Name { get; set; }
        public bool AutoJsonKnownType { get; set; }
    }
}
