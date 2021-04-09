using System;
using System.Threading;
using JsonKnownTypes.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonKnownTypes
{
    public class JsonKnownTypesReaderFactory
    {
        public JsonKnownTypesReaderFactory()
        { }

        public JsonKnownProxyReader GetOrCreate(JsonReader reader)
        {
            switch (reader)
            {
                case JsonKnownProxyReader pr:
                    return pr;
                default:
                    return new JsonKnownProxyReader(reader);
            }
        }
    }
    
    /// <summary>
    /// Convert json using discriminator
    /// </summary>
    public class JsonKnownTypesConverter<T> : JsonConverter
    {
        private static readonly DiscriminatorValues TypesDiscriminatorValues;
        private static readonly JsonKnownTypesReaderFactory JsonKnownTypesReaderFactory;

        static JsonKnownTypesConverter()
        {
            JsonKnownTypesReaderFactory = new JsonKnownTypesReaderFactory();
            TypesDiscriminatorValues = JsonKnownTypesSettingsManager.GetDiscriminatorValues<T>();
            JsonKnownTypesCache.TypeToDiscriminator.TryAdd(typeof(T), TypesDiscriminatorValues.FieldName);
        }
        
        public override bool CanConvert(Type objectType)
            => TypesDiscriminatorValues.Contains(objectType);

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

        protected virtual string ReadDiscriminator(JsonKnownProxyReader reader, string discriminatorFieldName)
        {
            var depth = reader.Depth + 1;
            string? discriminator = null;
            if (reader.Buffer.Count > 0)
            {
                var i = 0;
                while (i < reader.Buffer.Count)
                {
                    var info = reader.Buffer[i];
                    
                    if(depth > info.Depth)
                        break;

                    if (info.TokenType == JsonToken.PropertyName && depth == info.Depth)
                    {
                        if ((string?) info.Value == TypesDiscriminatorValues.FieldName)
                        {
                            if (i + 1 > reader.Buffer.Count)
                                break;
                            
                            discriminator = (string?)reader.Buffer[i + 1].Value;
                            break;
                        }
                    }

                    i++;
                } 
            }
            else
            {
                while (true)
                {
                    if (reader.TokenType == JsonToken.PropertyName && depth == reader.Depth)
                    {
                        if ((string?) reader.Value == TypesDiscriminatorValues.FieldName)
                        {
                            reader.ReadAndBuffer();
                            discriminator = (string) reader.Value;
                            reader.ReadAndBuffer();
                            break;
                        }
                    }

                    if(!reader.ReadAndBuffer())
                        break;
                
                    if(depth > reader.Depth)
                        break;
                }
            }
            
            reader.SetCursorToFirstInBuffer();
            return discriminator;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartObject) return null;

            var proxyReader = JsonKnownTypesReaderFactory.GetOrCreate(reader);
            var discriminator = ReadDiscriminator(proxyReader, TypesDiscriminatorValues.FieldName);

            if (TypesDiscriminatorValues.TryGetType(discriminator, out var typeForObject))
            {
                if (objectType == typeForObject)
                    _isInRead.Value = true;

                try
                {
                    var obj = serializer.Deserialize(proxyReader, typeForObject);
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

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var objectType = value.GetType();

            if (TypesDiscriminatorValues.IsFallback(objectType))
            {
                _isInWrite.Value = true;
                try
                {
                    serializer.Serialize(writer, value, objectType);
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

                JsonKnownProxyWriter writerProxy;
                if (writer is JsonKnownProxyWriter r)
                    writerProxy = r;
                else
                    writerProxy = new JsonKnownProxyWriter(writer);
                
                writerProxy.SetDiscriminator(TypesDiscriminatorValues.FieldName, discriminator);
                
                try
                {
                    serializer.Serialize(writerProxy, value, objectType);
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
