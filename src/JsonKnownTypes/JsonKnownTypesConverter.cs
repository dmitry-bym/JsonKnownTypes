﻿using System;
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

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            
            var jObject = JObject.Load(reader);

            var discriminator = jObject[_typesDiscriminatorValues.FieldName]?.Value<string>();

            if(string.IsNullOrEmpty(discriminator))
                throw new JsonKnownTypesException($"There is no discriminator fields with {_typesDiscriminatorValues.FieldName} name in json string or it is empty.");

            if (_typesDiscriminatorValues.TryGetType(discriminator, out var typeForObject))
            {
                var jsonReader = jObject.CreateReader();
                var target = _typesDiscriminatorValues.CreateInstance(typeForObject);

                serializer.Populate(jsonReader, target);
                return target;
            }

            throw new JsonKnownTypesException($"{discriminator} discriminator is not registered for {nameof(T)} type");
        }

        private readonly ThreadLocal<bool> _isInWrite = new ThreadLocal<bool>();

        public override bool CanWrite { get => !_isInWrite.Value; }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var objectType = value.GetType();
            if (_typesDiscriminatorValues.TryGetDiscriminator(objectType, out var discriminator))
            {
                _isInWrite.Value = true;

                var writerProxy = new JsonKnownProxyWriter(_typesDiscriminatorValues.FieldName, discriminator, writer);
                serializer.Serialize(writerProxy, value, objectType);

                _isInWrite.Value = false;
            }
            else
            {
                throw new JsonKnownTypesException($"There is no discriminator for {objectType.Name} type");
            }
        }
    }
}
