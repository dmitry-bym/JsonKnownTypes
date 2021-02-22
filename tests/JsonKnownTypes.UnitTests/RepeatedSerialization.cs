using Newtonsoft.Json;
using NUnit.Framework;
using System.IO;

namespace JsonKnownTypes.UnitTests
{
    public class RepeatedSerialization
    {
        [JsonDiscriminator(Name = "Type")]
        public class BaseType
        {
            public int BaseField { get; set; }
        }

        [JsonKnownThisType]
        public class DerivedType : BaseType
        {
            public int DerivedField { get; set; }
        }

        public class MemberLevelDesializerTestClass
        {
            [JsonConverter(typeof(JsonKnownTypesConverter<BaseType>))]
            public BaseType BaseTypeProperty { get; set; }
        }

        private string Serialize<T>(JsonSerializer serializer, T instance)
        {
            using (var writer = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    serializer.Serialize(jsonWriter, instance);
                }

                return writer.ToString();
            }
        }

        private T Deserialize<T>(JsonSerializer serializer, string serialized)
        {
            using (var reader = new StringReader(serialized))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<T>(jsonReader);
                }
            }
        }

        [Test]
        public void MemberLevelSharedSerializer()
        {
            var jsonSerializer = new JsonSerializer();

            for (int count = 0; count < 5; count++)
            {
                var serialized = Serialize(jsonSerializer, new MemberLevelDesializerTestClass { BaseTypeProperty = new DerivedType { BaseField = count, DerivedField = count } });

                Assert.DoesNotThrow(() => Deserialize<MemberLevelDesializerTestClass>(jsonSerializer, serialized));
            }
        }

        [Test]
        public void MemberLevelNonSharedSerializer()
        {
            for (int count = 0; count < 5; count++)
            {
                var serialized = JsonConvert.SerializeObject(new MemberLevelDesializerTestClass { BaseTypeProperty = new DerivedType { BaseField = count, DerivedField = count } });

                Assert.DoesNotThrow(() => JsonConvert.DeserializeObject<MemberLevelDesializerTestClass>(serialized));
            }
        }
    }
}
