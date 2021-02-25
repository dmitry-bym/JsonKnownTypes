namespace JsonKnownTypes
{
    public class JsonDiscriminatorSettings
    {
        /// <summary>
        /// Discriminator field name in json representation
        /// </summary>
        public string DiscriminatorFieldName { get; set; } = "$type";

        /// <summary>
        /// Add all types regardless of whether they're decorated with JsonKnownType attributes
        /// </summary>
        public bool AddAutoDiscriminators { get; set; } = true;

        /// <summary>
        /// If no descriminator for provided for a given type (i.e. through the <see cref="JsonKnownThisTypeAttribute"/>
        /// attribute then, if this setting is true then the <see cref="JsonKnownTypesConverter{T}"/> will look for
        /// a descriminator value from base types.
        /// </summary>
        /// <remarks>
        /// To use this setting effectively, <see cref="AddAutoDiscriminators"/> should be false and
        /// derived types should not be decorated with <see cref="JsonKnownThisTypeAttribute"/>
        /// </remarks>
        public bool UseBaseTypeDescriminators { get; set; } = false;
    }
}
