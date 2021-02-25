using System;

namespace JsonKnownTypes
{
    internal static class AttributesManager
    {
        internal static JsonKnownTypeAttribute[] GetJsonKnownAttributes(Type type) =>
            (JsonKnownTypeAttribute[])Attribute.GetCustomAttributes(type, typeof(JsonKnownTypeAttribute));

        internal static JsonKnownThisTypeAttribute GetJsonKnownThisAttribute(Type type) =>
            (JsonKnownThisTypeAttribute)Attribute.GetCustomAttribute(type, typeof(JsonKnownThisTypeAttribute));

        internal static JsonDiscriminatorAttribute GetJsonDiscriminatorAttribute(Type type) =>
            (JsonDiscriminatorAttribute)Attribute.GetCustomAttribute(type, typeof(JsonDiscriminatorAttribute));
        
        internal static JsonKnownTypeFallbackAttribute GetJsonKnownTypeFallbackAttribute(Type type) =>
            (JsonKnownTypeFallbackAttribute)Attribute.GetCustomAttribute(type, typeof(JsonKnownTypeFallbackAttribute));
    }
}
