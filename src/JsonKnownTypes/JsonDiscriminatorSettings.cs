namespace JsonKnownTypes
{
    public class JsonDiscriminatorSettings
    {
        /// <summary>
        /// Discriminator field name in json representation
        /// </summary>
        public string DiscriminatorFieldName { get; set; } = "$type";

        /// <summary>
        /// Use class name as discriminator if JsonKnown attribute hasn't been added
        /// </summary>
        public bool UseClassNameAsDiscriminator { get; set; } = true;
        
        /// <summary>
        /// Use JsonKnownTypesConverter with base class ONLY https://github.com/dmitry-bym/JsonKnownTypes/issues/26
        /// </summary>
        public bool UseBaseTypeForCanConvert { get; set; } = false;
    }
}
