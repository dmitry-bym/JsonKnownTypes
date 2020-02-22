using AutoFixture.NUnit3;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    [TestFixture]
    public class WithBaseAbstractClass
    {
        private static string DiscriminatorName { get => "type"; }

        [Test]
        [AutoData]
        public void AutoBaseAbstractClass1Heir(BaseAbstractClass1Heir entity)
            => There_is_right_discriminator(entity);

        [Test]
        [AutoData]
        public void AutoBaseAbstractClass2Heir(BaseAbstractClass2Heir entity)
            => There_is_right_discriminator(entity);

        private void There_is_right_discriminator(BaseAbstractClass entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var jObject = JToken.Parse(json);

            var discriminator = jObject[DiscriminatorName]?.Value<string>();
            var obj = JsonConvert.DeserializeObject<BaseAbstractClass>(json);

            obj.Should().BeEquivalentTo(entity);
            Assert.AreEqual(discriminator, entity.GetType().Name);
        }

        [Test]
        public void Settings_are_correct()
        {
            var settings = JsonKnownTypesSettingsManager.GetSettings<BaseAbstractClass>();

            Assert.True(settings.TypeToDiscriminator.Count == 2);
            Assert.AreEqual(settings.Name, DiscriminatorName);
        }
    }

    [JsonConverter(typeof(JsonKnownTypesConverter<BaseAbstractClass>))]
    [JsonDiscriminator(Name = "type")]
    [JsonKnownType(typeof(BaseAbstractClass1Heir))]
    [JsonKnownType(typeof(BaseAbstractClass2Heir))]
    public abstract class BaseAbstractClass
    {
        public string Summary { get; set; }
    }

    public class BaseAbstractClass1Heir : BaseAbstractClass
    {
        public int SomeInt { get; set; }
    }

    public class BaseAbstractClass2Heir : BaseAbstractClass
    {
        public double SomeDouble { get; set; }
        public string Detailed { get; set; }
    }
}
