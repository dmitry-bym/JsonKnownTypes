using JsonKnownTypes.Exceptions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    [TestFixture]
    public class DeserializeTest
    {
        [Test]
        [TestCase("{\"field\":\"value\"}")]
        [TestCase("{\"field\":\"value\", \"disc\":\"\"}")]
        public void Deserialize_without_discriminator(string json)
        {
            Assert.Throws<JsonKnownTypesException>(delegate
            {
                JsonConvert.DeserializeObject<DeserializeTestClass>(json);
            });
        }


        [JsonConverter(typeof(JsonKnownTypesConverter<DeserializeTestClass>))]
        [JsonKnownType(typeof(DeserializeTestClass), "disc")]
        class DeserializeTestClass
        {
            public string field { get; set; }
        } 
    }
}
