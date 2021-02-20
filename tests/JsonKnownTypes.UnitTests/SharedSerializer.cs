using Newtonsoft.Json;
using NUnit.Framework;
using System.IO;

namespace JsonKnownTypes.UnitTests
{
    public class SharedSerializer
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

        public class MemberDesierlizeTestClass
        {
            [JsonConverter(typeof(JsonKnownTypesConverter<BaseType>))]
            public BaseType BaseType { get; set; }
        }

        [Test]
        public void RepeatedSerialization()
        {
            var jsonSerializer = new JsonSerializer();

            for (int count = 0; count < 5; count++)
            {
                var serialized = Serialize(jsonSerializer, new MemberDesierlizeTestClass { BaseType = new DerivedType { BaseField = count, DerivedField = count } });

                Assert.DoesNotThrow(() => Deserialize(jsonSerializer, serialized));
            }
        }

        private string Serialize(JsonSerializer serializer, MemberDesierlizeTestClass memberDesierlizeTestClass)
        {
            using (var writer = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    serializer.Serialize(jsonWriter, memberDesierlizeTestClass);
                }

                return writer.ToString();
            }
        }

        private MemberDesierlizeTestClass Deserialize(JsonSerializer serializer, string serialized)
        {
            using (var reader = new StringReader(serialized))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<MemberDesierlizeTestClass>(jsonReader);
                }
            }
        }
    }
}
