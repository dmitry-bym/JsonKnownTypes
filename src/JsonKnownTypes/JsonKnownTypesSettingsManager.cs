using System;
using System.Linq;
using JsonKnownTypes.Utils;

namespace JsonKnownTypes
{
    public static class JsonKnownTypesSettingsManager<T>
    {
        /// <summary>
        /// Type specific default settings for discriminator
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType - Justification: Intentional property should exist on a per-type base
        public static JsonDiscriminatorSettings DefaultDiscriminatorSettings { get; } = new();
    }

    public static class JsonKnownTypesSettingsManager
    {
        /// <summary>
        /// Global Default settings for discriminator
        /// </summary>
        public static JsonDiscriminatorSettings DefaultDiscriminatorSettings { get; set; } = new();

        /// <summary>
        /// Function for search derived classes (By default in base class assembly only)
        /// </summary>
        public static Func<Type, Type[]> GetDerivedByBase = 
            parent => parent.Assembly.GetTypes();

        internal static DiscriminatorValues GetDiscriminatorValues<T>()
        {
            var discriminatorAttribute = AttributesManager.GetJsonDiscriminatorAttribute(typeof(T));

            var discriminatorSettings = discriminatorAttribute == null
                ? JsonKnownTypesSettingsManager<T>.DefaultDiscriminatorSettings
                  ??  DefaultDiscriminatorSettings
                : Mapper.Map(discriminatorAttribute);

            var typeSettings = new DiscriminatorValues(typeof(T), discriminatorSettings.DiscriminatorFieldName);

            typeSettings.AddJsonKnown<T>();

            var allTypes = GetFilteredDerived<T>();
            typeSettings.AddJsonKnownThis(allTypes);

            if (discriminatorSettings.UseClassNameAsDiscriminator)
            {
                typeSettings.AddAutoDiscriminators(allTypes, discriminatorSettings.NameDiscriminatorResolver);
            }

            return typeSettings;
        }

        private static Type[] GetFilteredDerived<T>()
        {
            var type = typeof(T);
            return GetDerivedByBase(type)
                .Where(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .ToArray();
        }
    }
}
