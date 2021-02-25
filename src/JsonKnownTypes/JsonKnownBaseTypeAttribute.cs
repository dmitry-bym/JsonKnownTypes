using System;

namespace JsonKnownTypes
{
    /// <summary>
    /// Add discriminator to type which is used with
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class JsonKnownBaseTypeAttribute : Attribute
    {
        public Type BaseType { get; }

        public JsonKnownBaseTypeAttribute()
        { }

        public JsonKnownBaseTypeAttribute(Type baseType)
        {
            BaseType = baseType;
        }
    }
}
