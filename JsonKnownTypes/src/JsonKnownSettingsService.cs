using System;
using System.Collections.Generic;
using System.Linq;
using JsonKnownTypes.Attributes;
using JsonKnownTypes.Exceptions;

namespace JsonKnownTypes
{
    public static class JsonKnownSettingsService
    {
        public static JsonKnownDiscriminatorAttribute DiscriminatorAttribute { get; set; } = new JsonKnownDiscriminatorAttribute();

        public static JsonKnownTypeSettings GetSettings<T>()
        {
            var data = new JsonKnownTypeSettings();
            var knownDiscriminatorAttribute = (JsonKnownDiscriminatorAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(JsonKnownDiscriminatorAttribute)) 
                                              ?? DiscriminatorAttribute;
            
            data.Name = knownDiscriminatorAttribute.Name;
            var autoType = knownDiscriminatorAttribute.AutoJsonKnownType;

            var knownTypeAttributes = (JsonKnownTypeAttribute[])Attribute.GetCustomAttributes(typeof(T), typeof(JsonKnownTypeAttribute));
            try
            {
                data.TypeToDiscriminator = knownTypeAttributes.ToDictionary(x => x.Type, x => x.Discriminator);

                if(autoType)
                    AddTypesWhichAreNotExists<T>(data.TypeToDiscriminator);

                data.DiscriminatorToType = data.TypeToDiscriminator.ToDictionary(x => x.Value, x => x.Key);
            }
            catch (ArgumentException e)
            {
                throw new AttributeArgumentException(e.Message, typeof(JsonKnownTypeAttribute).Name);
            }

            return data;
        }

        private static void AddTypesWhichAreNotExists<T>(Dictionary<Type, string> typeToDiscriminator)
        {
            foreach (var heir in GetAllInheritance<T>())
            {
                if(!typeToDiscriminator.ContainsKey(heir))
                    typeToDiscriminator.Add(heir, heir.Name);
            }
        }

        private static IEnumerable<Type> GetAllInheritance<T>()
        {
            var type = typeof(T);
            return type.Assembly
                .GetTypes()
                .Where(x => type.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
        }
    }
}
