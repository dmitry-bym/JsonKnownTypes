using System;

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
        /// Use a type property as name resolver. Default: type.Name
        /// </summary>
        public Func<Type, string> NameDiscriminatorResolver { get; set; } = type => type.Name;
    }
}