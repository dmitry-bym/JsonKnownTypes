using System;
using System.Linq;
using JsonKnownTypes.Exceptions;
using JsonKnownTypes.Utils;

namespace JsonKnownTypes
{
    public static class JsonKnownTypesSettingsManager
    {
        public static JsonDiscriminatorSettings DefaultDiscriminatorSettings { get; set; } =
            new JsonDiscriminatorSettings
            {
                DiscriminatorFieldName = "$type",
                UseClassNameAsDiscriminator = true
            };

        internal static DiscriminatorValues GetDiscriminatorValues<T>()
        {
            var discriminatorAttribute = AttributesManager.GetJsonDiscriminatorAttribute(typeof(T));

            var discriminatorSettings = discriminatorAttribute == null ? DefaultDiscriminatorSettings : Mapper.Map(discriminatorAttribute);

            var typeSettings = new DiscriminatorValues(discriminatorSettings.DiscriminatorFieldName);

            typeSettings.AddJsonKnown<T>();
            var allTypes = GetAllInheritance<T>();

            typeSettings.AddJsonKnownThis(allTypes);

            if (discriminatorSettings.UseClassNameAsDiscriminator)
            {
                typeSettings.AddAutoDiscriminators(allTypes);
            }
            else if (!allTypes.All(typeSettings.Contains))
            {
                var missingTypes = allTypes.Where(x => !typeSettings.Contains(x)).Select(x => x.Name);

                throw new JsonKnownTypesException($"Not all classes registered for { nameof(T) } type hierarchy." +
                                                  "Enable UseClassNameAsDiscriminator or add JsonKnown attributes for all classes." +
                                                  $"Missing classes: { string.Join(", ", missingTypes) }");
            }

            return typeSettings;
        }

        private static Type[] GetAllInheritance<T>()
        {
            var type = typeof(T);
            return type.Assembly
                .GetTypes()
                .Where(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToArray();
        }
    }
}
