using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonKnownTypes
{
    public class JsonKnownTypeConverter<T> : JsonConverter
    {
        private JsonKnownTypeSettings Settings { get; }

        private JsonSerializerSettings SpecifiedSubclassConversion
        {
            get =>
                new JsonSerializerSettings
                {
                    ContractResolver = new JsonKnownTypeContractResolver<T>()
                };
        }

        public JsonKnownTypeConverter()
        {
            Settings = JsonKnownSettingsService.GetSettings<T>();
        }

        public override bool CanConvert(Type objectType)
            => Settings.TypeToDiscriminator.ContainsKey(objectType);

        public override bool CanWrite { get => true; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            var discriminator = jo[Settings.Name].ToString();

            if(Settings.DiscriminatorToType.TryGetValue(discriminator, out var typeForObject))
                return JsonConvert.DeserializeObject(jo.ToString(), typeForObject, SpecifiedSubclassConversion);

            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var objectType = value.GetType();
            if (Settings.TypeToDiscriminator.TryGetValue(objectType, out var discriminator))
            {
                var json = JsonConvert.SerializeObject(value, SpecifiedSubclassConversion);
                var jo = JObject.Parse(json);
                jo.Add(Settings.Name, new JValue(discriminator));
                jo.WriteTo(writer);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
