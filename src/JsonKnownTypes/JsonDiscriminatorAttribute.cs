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
        /// If no descriminator for provided for a given type (i.e. through the <see cref="JsonKnownThisTypeAttribute"/>
        /// attribute then, if this setting is true then the <see cref="JsonKnownTypesConverter{T}"/> will look for
        /// a descriminator value from base types.
        /// </summary>
        /// <remarks>
        /// To use this setting effectively, <see cref="AddAutoDiscriminators"/> should be false and
        /// derived types should not be decorated with <see cref="JsonKnownThisTypeAttribute"/>
        /// </remarks>
        public bool UseBaseTypeDescriminators
        {
            get => UseBaseTypeDescriminatorsValue != false;
            set => UseBaseTypeDescriminatorsValue = value;
        }

        internal bool? UseBaseTypeDescriminatorsValue;

        /// <summary>
        /// Use class name as discriminator if JsonKnown attribute didn't add
        /// </summary>
        public bool AddAutoDiscriminators
        {
            get => AutoJson != false;
            set => AutoJson = value;
        }

        internal bool? AutoJson;
    }
}
