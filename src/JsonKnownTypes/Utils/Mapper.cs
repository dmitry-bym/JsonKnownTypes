namespace JsonKnownTypes.Utils
{
    internal static class Mapper
    {
        public static JsonDiscriminatorSettings Map(JsonDiscriminatorAttribute entity)
            => new JsonDiscriminatorSettings
            {
                Name = entity.Name,
                AutoJsonKnownType = entity.AutoJsonKnownType
            };
    }
}
