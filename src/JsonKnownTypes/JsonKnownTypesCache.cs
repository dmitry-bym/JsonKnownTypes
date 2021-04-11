using System;
using System.Collections.Concurrent;

namespace JsonKnownTypes
{
    
    public static class JsonKnownTypesCache
    {
        /// <summary>
        /// Dictionary with base type as key and discriminator field name as value
        /// </summary>
        public static readonly ConcurrentDictionary<Type, string> TypeToDiscriminator 
            = new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// Bag with all discriminator values
        /// </summary>
        public static readonly ConcurrentBag<DiscriminatorValues> DiscriminatorValues = 
            new ConcurrentBag<DiscriminatorValues>();
    }
}
