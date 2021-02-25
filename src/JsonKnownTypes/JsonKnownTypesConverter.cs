using System;
using System.Threading;
using JsonKnownTypes.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonKnownTypes
{
    /// <summary>
    /// Convert json using discriminator
    /// </summary>
    public class JsonKnownTypesConverter<T> : JsonConverter
    {
        private readonly DiscriminatorValues _typesDiscriminatorValues
            = JsonKnownTypesSettingsManager.GetDiscriminatorValues<T>();

        public override bool CanConvert(Type objectType)
            => _typesDiscriminatorValues.Contains(objectType);

        private readonly ThreadLocal<bool> _isInRead = new ThreadLocal<bool>();

        public override bool CanRead => !IsInReadAndReset();

        private bool IsInReadAndReset()
        {
            if (_isInRead.Value)
            {
                _isInRead.Value = false;
                return true;
            }
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            var jObject = JObject.Load(reader);

            var discriminator = jObject[_typesDiscriminatorValues.FieldName]?.Value<string>();

            if (_typesDiscriminatorValues.TryGetType(discriminator, out var typeForObject))
            {
                var jsonReader = jObject.CreateReader();

                if (objectType == typeForObject)
                    _isInRead.Value = true;

                try
                {
                    var obj = serializer.Deserialize(jsonReader, typeForObject);

                    return obj;
                }
                finally
                {
                    _isInRead.Value = false;
                }
            }

            var discriminatorName = string.IsNullOrWhiteSpace(discriminator) ? "<empty-string>" : discriminator;
            throw new JsonKnownTypesException($"{discriminatorName} discriminator is not registered for {nameof(T)} type");
        }

        private readonly ThreadLocal<bool> _isInWrite = new ThreadLocal<bool>();

        public override bool CanWrite => !IsInWriteAndReset();

        private bool IsInWriteAndReset()
        {
            if (_isInWrite.Value)
            {
                _isInWrite.Value = false;
                return true;
            }
            return false;
        }

        private bool TryGetDiscriminator(Type objectType, bool useBaseTypeDescriminators, out string discriminator)
        {
            if (_typesDiscriminatorValues.TryGetDiscriminator(objectType, out discriminator))
            {
                return true;
            }
            else if (objectType.BaseType != null && useBaseTypeDescriminators)
            {
                return TryGetDiscriminator(objectType.BaseType, useBaseTypeDescriminators, out discriminator);
            }
            else
            {
                return false;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var objectType = value.GetType();

            if (_typesDiscriminatorValues.FallbackType != null && objectType == _typesDiscriminatorValues.FallbackType)
            {
                _isInWrite.Value = true;

                try
                {
                    serializer.Serialize(writer, value, objectType);
                    return;
                }
                finally
                {
                    _isInWrite.Value = false;
                }
            }

            if (TryGetDiscriminator(objectType, _typesDiscriminatorValues.UseBaseTypeDescriminators, out var discriminator))
            {
                try
                {
                    _isInWrite.Value = true;

                    var writerProxy = new JsonKnownProxyWriter(_typesDiscriminatorValues.FieldName, discriminator, writer);
                    serializer.Serialize(writerProxy, value, objectType);
                }
                finally
                {
                    _isInWrite.Value = false;
                }
            }
            else
            {
                throw new JsonKnownTypesException($"There is no discriminator for {objectType.Name} type. If {objectType.Name} is derived from a type that provides a discriminator value then you can try adding 'UseBaseTypeDescriminators = true' to the 'JsonDiscriminator' attribute for the type '{typeof(T).Name}'");
            }
        }
    }
}
