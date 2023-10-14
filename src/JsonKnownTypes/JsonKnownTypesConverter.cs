using System;
using System.Threading;
using JsonKnownTypes.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonKnownTypes
{
    /// <summary>
    /// Convert an object with discriminator to and from JSON
    /// </summary>
    public class JsonKnownTypesConverter<T> : JsonConverter
    {
        private static readonly DiscriminatorValues TypesDiscriminatorValues;

        static JsonKnownTypesConverter()
        {
            TypesDiscriminatorValues = JsonKnownTypesSettingsManager.GetDiscriminatorValues<T>();
            JsonKnownTypesCache.TypeToDiscriminator.TryAdd(typeof(T), TypesDiscriminatorValues.FieldName);
            JsonKnownTypesCache.DiscriminatorValues.Add(TypesDiscriminatorValues);
        }
        
        public override bool CanConvert(Type objectType)
        {
            return TypesDiscriminatorValues.CanConvert(objectType);
        }

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

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            var jObject = JObject.Load(reader);

            var discriminator = jObject[TypesDiscriminatorValues.FieldName]?.Value<string>();

            if (TypesDiscriminatorValues.TryGetType(discriminator, out var typeForObject))
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
            throw new JsonKnownTypesException(
                $"{discriminatorName} discriminator is not registered for {nameof(T)} type");
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

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var objectType = value.GetType();

            if (TypesDiscriminatorValues.FallbackType != null && objectType == TypesDiscriminatorValues.FallbackType)
            {
                _isInWrite.Value = true;
                try
                {
                    serializer.Serialize(writer, value);
                }
                finally
                {
                    _isInWrite.Value = false;
                }

                return;
            }

            if (TypesDiscriminatorValues.TryGetDiscriminator(objectType, out var discriminator))
            {
                _isInWrite.Value = true;

                var writerProxy = new JsonKnownProxyWriter(TypesDiscriminatorValues.FieldName, discriminator, writer);
                
                try
                {
                    serializer.Serialize(writerProxy, value);
                }
                finally
                {
                    _isInWrite.Value = false;
                }
            }
            else
            {
                throw new JsonKnownTypesException($"There is no discriminator for {objectType.Name} type");
            }
        }
    }
}
