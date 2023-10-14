namespace JsonKnownTypes.Utils
{
    internal static class Mapper
    {
        internal static JsonDiscriminatorSettings Map(JsonDiscriminatorAttribute entity, JsonDiscriminatorSettings defaultSettings)
        {
            var settings = new JsonDiscriminatorSettings
            {
                DiscriminatorFieldName = entity.Name ?? defaultSettings.DiscriminatorFieldName,
                UseClassNameAsDiscriminator = entity.AutoJson ?? defaultSettings.UseClassNameAsDiscriminator,
                UseBaseTypeForCanConvert = entity.UseBaseType ?? defaultSettings.UseBaseTypeForCanConvert
            };

            return settings;
        }
    }
}
