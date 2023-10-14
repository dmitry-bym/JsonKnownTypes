using System;

namespace JsonKnownTypes.Exceptions
{
    public class JsonKnownTypesException : Exception
    {
        internal JsonKnownTypesException(string message) 
            : base(message)
        { }
    }
}
