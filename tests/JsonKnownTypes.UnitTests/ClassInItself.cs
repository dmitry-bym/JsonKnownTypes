using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    public class ClassInItself
    {
        [Test]
        public void BaseClassTest()
        {
            var baseClass = new BaseClass {FieldBaseClass = new BaseClassHeir {FieldBaseClass = null, Summary = "aaa"}};
            There_is_right_discriminator(baseClass);
        }

        private void There_is_right_discriminator(BaseClass entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var jObject = JToken.Parse(json);

            var discriminator = jObject["$type"]?.Value<string>();
            var obj = JsonConvert.DeserializeObject<BaseClass>(json);

            obj.Should().BeEquivalentTo(entity);
            Assert.AreEqual(discriminator, entity.GetType().Name);
        }

        [JsonConverter(typeof(JsonKnownTypesConverter<BaseClass>))]
        [JsonDiscriminator(UseClassNameAsDiscriminator = false)]
        [JsonKnownThisType]
        public class BaseClass
        {
            public BaseClass FieldBaseClass { get; set; }
        }

        [JsonKnownThisType]
        public class BaseClassHeir : BaseClass
        {
            public string Summary { get; set; }
        }

    }
}
