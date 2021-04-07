using System;
using System.Collections.Concurrent;

namespace JsonKnownTypes
{
    public static class JsonKnownTypesCache
    {
        public static ConcurrentDictionary<Type, string> TypeToDiscriminator 
            = new ConcurrentDictionary<Type, string>();

        public static ConcurrentBag<DiscriminatorValues> DiscriminatorValues = 
            new ConcurrentBag<DiscriminatorValues>();
    }
}
