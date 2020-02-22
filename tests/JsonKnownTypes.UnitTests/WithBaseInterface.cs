using AutoFixture.NUnit3;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    [TestFixture]
    public class WithBaseInterface
    {
        private static string DiscriminatorName { get => "$type"; }

        [Test]
        [AutoData]
        public void AutoBaseInterface1Heir(BaseInterface1Heir entity)
            => There_is_right_discriminator(entity);

        [Test]
        [AutoData]
        public void AutoBaseInterface2Heir(BaseInterface2Heir entity)
            => There_is_right_discriminator(entity);

        private void There_is_right_discriminator(IBaseInterface entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var jObject = JToken.Parse(json);

            var discriminator = jObject[DiscriminatorName]?.Value<string>();
            var obj = JsonConvert.DeserializeObject<IBaseInterface>(json);

            obj.Should().BeEquivalentTo(entity);
            Assert.AreEqual(discriminator, entity.GetType().Name);
        }
    }

    [JsonConverter(typeof(JsonKnownTypesConverter<IBaseInterface>))]
    [JsonKnownType(typeof(BaseInterface2Heir))]
    public interface IBaseInterface
    {
        string Summary { get; set; }
    }

    public class BaseInterface1Heir : IBaseInterface
    {
        public string Summary { get; set; }
    }

    public class BaseInterface2Heir : IBaseInterface
    {
        public string Summary { get; set; }
        public string Detailed { get; set; }
    }
}
