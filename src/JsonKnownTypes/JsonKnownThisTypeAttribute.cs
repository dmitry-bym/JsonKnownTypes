using System;

namespace JsonKnownTypes
{
    /// <summary>
    /// Add discriminator to type which is used with
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class JsonKnownThisTypeAttribute : Attribute
    {
        public string Discriminator { get; }

        public JsonKnownThisTypeAttribute()
        { }

        public JsonKnownThisTypeAttribute(string discriminator)
        {
            Discriminator = discriminator;
        }
    }
}
