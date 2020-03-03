using System;
using JsonKnownTypes.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonKnownTypes
{
    public class JsonKnownTypesConverter<T> : JsonConverter
    {
        private readonly JsonKnownTypesSettings _typesSettings 
            = JsonKnownTypesSettingsManager.GetSettings<T>();

        private JsonSerializerSettings SpecifiedSubclassConversion
        {
            get =>
                new JsonSerializerSettings
                {
                    ContractResolver = new JsonKnownTypesContractResolver<T>()
                };
        }
        
        public override bool CanConvert(Type objectType)
            => _typesSettings.Contains(objectType);

        public override bool CanWrite { get => true; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            var discriminator = jo[_typesSettings.Name].ToString();

            if(_typesSettings.TryGetType(discriminator, out var typeForObject))
                return JsonConvert.DeserializeObject(jo.ToString(), typeForObject, SpecifiedSubclassConversion);

            throw new JsonKnownTypesException($"discriminator {discriminator} is not registered for {nameof(T)} base type");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var objectType = value.GetType();
            if (_typesSettings.TryGetDiscriminator(objectType, out var discriminator))
            {
                var json = JsonConvert.SerializeObject(value, SpecifiedSubclassConversion);
                var jo = JObject.Parse(json);
                jo.Add(_typesSettings.Name, new JValue(discriminator));
                jo.WriteTo(writer);
            }
            else
            {
                throw new JsonKnownTypesException($"there is no discriminator for {objectType.Name} type");
            }
        }
    }
}
