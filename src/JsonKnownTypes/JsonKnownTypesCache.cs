using System;
using System.Collections.Concurrent;

namespace JsonKnownTypes
{
    public static class JsonKnownTypesCache
    {
        public static ConcurrentDictionary<Type, string> TypeToDiscriminator 
            = new ConcurrentDictionary<Type, string>();
    }
}
