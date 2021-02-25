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
    }
}
