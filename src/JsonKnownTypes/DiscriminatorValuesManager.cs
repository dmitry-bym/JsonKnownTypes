using System;

namespace JsonKnownTypes
{
    internal static class DiscriminatorValuesManager
    {
        internal static void AddJsonKnown<T>(this DiscriminatorValues discriminatorValues)
        {
            var attrs = AttributesManager.GetJsonKnownAttributes(typeof(T));
            foreach (var attr in attrs)
            {
                var discriminator = attr.Discriminator ?? attr.Type.Name;
                discriminatorValues.AddType(attr.Type, discriminator);
            }

            var fallbackTypeAttribute = AttributesManager.GetJsonKnownTypeFallbackAttribute(typeof(T));
            if (fallbackTypeAttribute != null)
            {
                discriminatorValues.AddFallbackType(fallbackTypeAttribute.FallbackType);
            }
        }

        internal static void AddJsonKnownThis(this DiscriminatorValues discriminatorValues, Type[] inherited)
        {
            foreach (var type in inherited)
            {
                var attr = AttributesManager.GetJsonKnownThisAttribute(type);
                if (attr != null)
                {
                    var discriminator = attr.Discriminator ?? type.Name;
                    discriminatorValues.AddType(type, discriminator);
                }
            }
        }

        internal static void AddAutoDiscriminators(this DiscriminatorValues discriminatorValues, Type[] inherited)
        {
            foreach (var type in inherited)
            {
                if (discriminatorValues.Contains(type))
                    continue;

                discriminatorValues.AddType(type, type.Name);
            }
        }
    }
}
