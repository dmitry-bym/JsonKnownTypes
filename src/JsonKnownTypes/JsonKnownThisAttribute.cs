using System;

namespace JsonKnownTypes
{
    public class JsonKnownThisAttribute : Attribute
    {
        public string Discriminator { get; }

        public JsonKnownThisAttribute()
        { }

        public JsonKnownThisAttribute(string discriminator)
        {
            Discriminator = discriminator;
        }
    }
}
