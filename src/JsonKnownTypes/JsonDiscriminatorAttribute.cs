using System;

namespace JsonKnownTypes
{
    /// <summary>
    /// Manage discriminator settings
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class JsonDiscriminatorAttribute : Attribute
    {
        /// <summary>
        /// Discriminator field name in json representation
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Use JsonKnownTypesConverter with base class ONLY https://github.com/dmitry-bym/JsonKnownTypes/issues/26
        /// </summary>
        public bool UseBaseTypeForCanConvert
        {
            get => UseBaseType != false;
            set => UseBaseType = value;
        }

        internal bool? UseBaseType;
        
        
        /// <summary>
        /// Use class name as discriminator if JsonKnown attribute didn't add
        /// </summary>
        public bool UseClassNameAsDiscriminator
        {
            get => AutoJson != false;
            set => AutoJson = value;
        }

        internal bool? AutoJson;
    }
}
