using System;

namespace JsonKnownTypes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class JsonDiscriminatorAttribute : Attribute
    {
        public string Name { get; set; }

        internal bool? AutoJson;

        public bool UseClassNameAsDiscriminator
        {
            get => AutoJson != false;
            set => AutoJson = value;
        }
    }
}
