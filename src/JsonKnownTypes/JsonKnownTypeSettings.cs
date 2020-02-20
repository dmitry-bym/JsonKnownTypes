using System;
using System.Collections.Generic;

namespace JsonKnownTypes
{
    public class JsonKnownTypeSettings
    {
        public string Name { get; set; }
        public Dictionary<string, Type> DiscriminatorToType { get; set; }
        public Dictionary<Type, string> TypeToDiscriminator { get; set; }
    }
}
