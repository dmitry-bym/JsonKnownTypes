using AutoFixture.NUnit3;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    [TestFixture]
    public class WithBaseClass
    {
        private static string DiscriminatorName { get => "type"; }

        [Test]
        [AutoData]
        public void AutoBaseClass(BaseClass entity) 
            => There_is_right_discriminator(entity);

        [Test]
        [AutoData]
        public void AutoBaseClass1Heir(BaseClass1Heir entity)
            => There_is_right_discriminator(entity);

        [Test]
        [AutoData]
        public void AutoBaseClass2Heir(BaseClass2Heir entity)
            => There_is_right_discriminator(entity);

        [Test]
        [AutoData]
        public void AutoBaseClass3Heir(BaseClass3Heir entity)
            => There_is_right_discriminator(entity);

        private void There_is_right_discriminator(BaseClass entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var jObject = JToken.Parse(json);

            var discriminator = jObject[DiscriminatorName]?.Value<string>();
            var obj = JsonConvert.DeserializeObject<BaseClass>(json);

            obj.Should().BeEquivalentTo(entity);
            Assert.AreEqual(discriminator, entity.GetType().Name);
        }

        [Test]
        public void Settings_are_correct()
        {
            var settings = JsonKnownTypesSettingsManager.GetSettings<BaseClass>();

            Assert.True(settings.TypeToDiscriminator.Count == 4);
            Assert.AreEqual(settings.Name, DiscriminatorName);
        }
    }

    [JsonConverter(typeof(JsonKnownTypesConverter<BaseClass>))]
    [JsonDiscriminator(AutoJson = false, Name = "type")]
    [JsonKnownThisType]
    public class BaseClass
    {
        public string Summary { get; set; }
    }

    [JsonKnownThisType]
    public class BaseClass1Heir : BaseClass
    {
        public int SomeInt { get; set; }
    }

    [JsonKnownThisType]
    public class BaseClass2Heir : BaseClass
    {
        public double SomeDouble { get; set; }
        public string Detailed { get; set; }
    }

    [JsonKnownThisType]
    public class BaseClass3Heir : BaseClass2Heir
    {
        public string SomeString { get; set; }
    }
}
