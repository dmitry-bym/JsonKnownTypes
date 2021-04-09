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
        private static readonly DiscriminatorValues TypesDiscriminatorValues;

        static JsonKnownTypesConverter()
        {
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

        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            if (reader.TokenType != JsonToken.StartObject) return null;

            JsonKnownProxyReader proxyReader;
            if (reader is JsonKnownProxyReader r)
                proxyReader = r;
            else
                proxyReader = new JsonKnownProxyReader(reader);

            
            var depth = proxyReader.Depth + 1;
            string? discriminator = null;
            if (proxyReader.Buffer.Count > 0)
            {
                var i = 0;
                while (i < proxyReader.Buffer.Count)
                {
                    var info = proxyReader.Buffer[i];
                    
                    if(depth > info.Depth)
                        break;

                    if (info.TokenType == JsonToken.PropertyName && depth == info.Depth)
                    {
                        if ((string?) info.Value == TypesDiscriminatorValues.FieldName)
                        {
                            if (i + 1 > proxyReader.Buffer.Count)
                                break;
                            
                            discriminator = (string?)proxyReader.Buffer[i + 1].Value;
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
                    if (proxyReader.TokenType == JsonToken.PropertyName && depth == proxyReader.Depth)
                    {
                        if ((string?) proxyReader.Value == TypesDiscriminatorValues.FieldName)
                        {
                            proxyReader.ReadAndBuffer();
                            discriminator = (string) proxyReader.Value;
                            proxyReader.ReadAndBuffer();
                            break;
                        }
                    }

                    if(!proxyReader.ReadAndBuffer())
                        break;
                
                    if(depth > proxyReader.Depth)
                        break;
                }
            }
            

            if (TypesDiscriminatorValues.TryGetType(discriminator, out var typeForObject))
            {
                if (objectType == typeForObject)
                    _isInRead.Value = true;

                try
                {
                    proxyReader.SetCursorToFirstInBuffer();
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
