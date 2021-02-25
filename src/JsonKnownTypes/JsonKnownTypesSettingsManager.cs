using System;
using System.Linq;
using JsonKnownTypes.Utils;

namespace JsonKnownTypes
{
    public static class JsonKnownTypesSettingsManager
    {
        /// <summary>
        /// Default settings for discriminator
        /// </summary>
        public static JsonDiscriminatorSettings DefaultDiscriminatorSettings { get; set; } =
            new JsonDiscriminatorSettings();


        /// <summary>
        /// Function for search derived classes (By default just in base class assembly)
        /// </summary>
        public static Func<Type, Type[]> GetDerivedByBase = 
            parent => parent.Assembly.GetTypes();

        internal static DiscriminatorValues GetDiscriminatorValues<T>()
        {
            var discriminatorAttribute = AttributesManager.GetJsonDiscriminatorAttribute(typeof(T));

            var discriminatorSettings = discriminatorAttribute == null 
                ? DefaultDiscriminatorSettings 
                : Mapper.Map(discriminatorAttribute);

            var typeSettings = new DiscriminatorValues(discriminatorSettings.DiscriminatorFieldName, discriminatorSettings.UseBaseTypeDescriminators);

            typeSettings.AddJsonKnown<T>();

            var allTypes = GetFilteredDerived<T>();
            typeSettings.AddJsonKnownThis(allTypes);

            if (discriminatorSettings.AddAutoDiscriminators)
            {
                typeSettings.AddAutoDiscriminators(allTypes);
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
