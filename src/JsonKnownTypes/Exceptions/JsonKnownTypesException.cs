using System;

namespace JsonKnownTypes.Exceptions
{
    public class JsonKnownTypesException : Exception
    {
        public JsonKnownTypesException(string message) 
            : base(message)
        { }
    }
}
