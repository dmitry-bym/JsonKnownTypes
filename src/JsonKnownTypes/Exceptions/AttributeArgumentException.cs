using System;

namespace JsonKnownTypes.Exceptions
{
    public class AttributeArgumentException : ArgumentException //todo made cool exceptions
    {
        private string AttributeName { get; }

        public AttributeArgumentException()
        { }

        public AttributeArgumentException(string message, string attributeName)
            : base(message)
        {
            AttributeName = attributeName;
        }

        public AttributeArgumentException(string message, string paramName, string attributeName)
            : base(message, paramName)
        {
            AttributeName = attributeName;
        }
        
        public override string ToString() 
            => string.Concat("Attribute name is ", AttributeName, base.ToString());
    }
}
