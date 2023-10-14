using System;
using System.Collections.Generic;
using JsonKnownTypes.Exceptions;

namespace JsonKnownTypes
{
    public class DiscriminatorValues
    {
        public string FieldName { get; }
        public Type BaseType { get; }
        private readonly Dictionary<string, Type> _discriminatorToType;
        private readonly Dictionary<Type, string> _typeToDiscriminator;
        private readonly bool _useBaseTypeForCanConvert;

        public DiscriminatorValues(Type baseType, string fieldName, bool useBaseTypeForCanConvert)
        {
            BaseType = baseType;
            FieldName = fieldName;
            _useBaseTypeForCanConvert = useBaseTypeForCanConvert;
            _discriminatorToType = new Dictionary<string, Type>();
            _typeToDiscriminator = new Dictionary<Type, string>();
        }

        public bool CanConvert(Type type)
        {
            return _useBaseTypeForCanConvert ? type == BaseType : Contains(type);
        }

        public Type FallbackType { get; private set; }

        public int Count => _typeToDiscriminator.Count;

        public bool TryGetDiscriminator(Type type, out string discriminator)
            => _typeToDiscriminator.TryGetValue(type, out discriminator);

        public bool TryGetType(string discriminator, out Type type)
        {
            var discriminatorIsNull = discriminator == null;
            var fallbackTypeExists = FallbackType != null;

            Type existingType = null;
            // ReSharper disable once SimplifyConditionalTernaryExpression
            var typeExists = discriminatorIsNull
                ? false
                : _discriminatorToType.TryGetValue(discriminator, out existingType);

            switch (discriminatorIsNull, fallbackTypeExists, typeExists)
            {
                case (true, true, _):
                    type = FallbackType;
                    return true;

                case (false, _, true):
                    type = existingType;
                    return true;

                case (false, true, false):
                    type = FallbackType;
                    return true;

                default:
                    type = null;
                    return false;
            }
        }

        public bool Contains(Type type) => _typeToDiscriminator.ContainsKey(type);

        public bool Contains(string discriminator) => _discriminatorToType.ContainsKey(discriminator);

        public void AddType(Type type, string discriminator)
        {
            if (type == null)
                throw new JsonKnownTypesException("null passed as type");

            if (string.IsNullOrWhiteSpace(discriminator))
                throw new JsonKnownTypesException($"Invalid discriminator for {type} type");

            if (_typeToDiscriminator.ContainsKey(type))
                throw new JsonKnownTypesException($"{type} type already registered");

            if (_discriminatorToType.ContainsKey(discriminator))
                throw new JsonKnownTypesException($"{discriminator} discriminator already in use");

            _typeToDiscriminator.Add(type, discriminator);
            _discriminatorToType.Add(discriminator, type);
        }

        public void AddFallbackType(Type type)
        {
            if (FallbackType != null)
                throw new JsonKnownTypesException($"{FallbackType} fallback type is already registered");

            FallbackType = type;
        }
    }
}
