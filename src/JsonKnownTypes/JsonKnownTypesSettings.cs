using System;
using System.Collections.Generic;
using JsonKnownTypes.Exceptions;

namespace JsonKnownTypes
{
    public class JsonKnownTypesSettings
    {
        public string Name { get; }
        private Dictionary<string, Type> DiscriminatorToType { get; }
        private Dictionary<Type, string> TypeToDiscriminator { get; }

        public JsonKnownTypesSettings(string name)
        {
            Name = name;
            DiscriminatorToType = new Dictionary<string, Type>();
            TypeToDiscriminator = new Dictionary<Type, string>();
        }

        public int Count
        {
            get => TypeToDiscriminator.Count;
        }

        public bool TryGetDiscriminator(Type type, out string discriminator) 
            => TypeToDiscriminator.TryGetValue(type, out discriminator);

        public bool TryGetType(string discriminator, out Type type)
            => DiscriminatorToType.TryGetValue(discriminator, out type);

        public bool Contains(Type type) 
            => TypeToDiscriminator.ContainsKey(type);
        public bool Contains(string discriminator) 
            => DiscriminatorToType.ContainsKey(discriminator);

        public void AddType(Type type, string discriminator)
        {
            if (TypeToDiscriminator.ContainsKey(type))
                throw new JsonKnownTypesException($"{type} type already registered");

            if (DiscriminatorToType.ContainsKey(discriminator))
                throw new JsonKnownTypesException($"{discriminator} discriminator already in use");

            TypeToDiscriminator.Add(type, discriminator);
            DiscriminatorToType.Add(discriminator, type);
        }

        public void AddIfIsNotContainsType(Type type, string discriminator)
        {
            if (TypeToDiscriminator.ContainsKey(type))
                return;

            if (DiscriminatorToType.ContainsKey(discriminator))
                throw new JsonKnownTypesException($"{discriminator} discriminator already in use");

            TypeToDiscriminator.Add(type, discriminator);
            DiscriminatorToType.Add(discriminator, type);
        }
    }
}
