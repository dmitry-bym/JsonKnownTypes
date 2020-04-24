using System;
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

        private JsonSerializerSettings SpecifiedSubclassConversion
        {
            get =>
                new JsonSerializerSettings
                {
                    ContractResolver = new JsonKnownTypesContractResolver<T>()
                };
        }
        
        public override bool CanConvert(Type objectType)
            => _typesDiscriminatorValues.Contains(objectType);

        public override bool CanWrite { get => true; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            
            var jo = JObject.Load(reader);

            var discriminator = jo[_typesDiscriminatorValues.FieldName].ToString();

            if (_typesDiscriminatorValues.TryGetType(discriminator, out var typeForObject))
                return JsonConvert.DeserializeObject(jo.ToString(), typeForObject, SpecifiedSubclassConversion);

            throw new JsonKnownTypesException($"{discriminator} discriminator is not registered for {nameof(T)} type");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var objectType = value.GetType();
            if (_typesDiscriminatorValues.TryGetDiscriminator(objectType, out var discriminator))
            {
                var json = JsonConvert.SerializeObject(value, SpecifiedSubclassConversion);
                var jo = JObject.Parse(json);
                jo.Add(_typesDiscriminatorValues.FieldName, new JValue(discriminator));
                jo.WriteTo(writer);
            }
            else
            {
                throw new JsonKnownTypesException($"There is no discriminator for {objectType.Name} type");
            }
        }
    }
}
