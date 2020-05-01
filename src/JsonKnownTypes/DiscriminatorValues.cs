using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JsonKnownTypes.Exceptions;

namespace JsonKnownTypes
{
    internal class DiscriminatorValues
    {
        public string FieldName { get; }
        private Dictionary<string, Type> DiscriminatorToType { get; }
        private Dictionary<Type, string> TypeToDiscriminator { get; }
        private Dictionary<Type, Func<object>> TypeToConstructor { get; }

        public DiscriminatorValues(string fieldName)
        {
            FieldName = fieldName;
            DiscriminatorToType = new Dictionary<string, Type>();
            TypeToDiscriminator = new Dictionary<Type, string>();
            TypeToConstructor = new Dictionary<Type, Func<object>>();
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

            TypeToConstructor.Add(type, GenerateConstructor(type));
            TypeToDiscriminator.Add(type, discriminator);
            DiscriminatorToType.Add(discriminator, type);
        }

        
        public object CreateInstance(Type type) 
            => TypeToConstructor[type]();

        private static Func<object> GenerateConstructor(Type type)
        {
            var constructor = type.GetConstructors().FirstOrDefault(x => x.GetParameters().Length == 0) 
                      ?? throw new JsonKnownTypesException($"{type.Name} type has no constructor without parameters");

            var newExpr = Expression.New(constructor);

            var convertExpr = Expression.Convert(newExpr, typeof(object));
            var lambda = Expression.Lambda<Func<object>>(convertExpr);
            return lambda.Compile();
        }
    }
}
