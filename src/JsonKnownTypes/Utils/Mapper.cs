namespace JsonKnownTypes.Utils
{
    internal static class Mapper
    {
        internal static JsonDiscriminatorSettings Map(JsonDiscriminatorAttribute entity)
        {
            var settings = new JsonDiscriminatorSettings();

            settings.DiscriminatorFieldName = entity.Name ?? settings.DiscriminatorFieldName;
            settings.AddAutoDiscriminators = entity.AutoJson ?? settings.AddAutoDiscriminators;
            settings.UseBaseTypeDescriminators = entity.UseBaseTypeDescriminatorsValue ?? settings.UseBaseTypeDescriminators;
            
            return settings;
        }
    }
}
