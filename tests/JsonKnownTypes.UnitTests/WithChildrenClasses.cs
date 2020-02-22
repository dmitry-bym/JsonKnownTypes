using AutoFixture.NUnit3;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    [TestFixture]
    public class WithChildrenClasses
    {
        private static string DiscriminatorName { get => "$type"; }

        [Test]
        [AutoData]
        public void AutoParentClass(ParentClass entity)
            => There_is_right_discriminator(entity);

        [Test]
        [AutoData]
        public void AutoParentClass1Heir(ParentClass1Heir entity)
            => There_is_right_discriminator(entity);

        [Test]
        [AutoData]
        public void AutoParentClass2Heir(ParentClass2Heir entity)
            => There_is_right_discriminator(entity);

        private void There_is_right_discriminator(ParentClass entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var jObject = JToken.Parse(json);

            var discriminator = jObject[DiscriminatorName]?.Value<string>();
            var obj = JsonConvert.DeserializeObject<ParentClass>(json);

            obj.Should().BeEquivalentTo(entity);
            Assert.AreEqual(discriminator, entity.GetType().Name.ToLower());
        }

        [Test]
        public void Settings_are_correct()
        {
            var settings = JsonKnownTypesSettingsManager.GetSettings<ParentClass>();

            Assert.True(settings.TypeToDiscriminator.Count == 3);
            Assert.AreEqual(settings.Name, DiscriminatorName);
        }
    }

    [JsonConverter(typeof(JsonKnownTypesConverter<ParentClass>))]
    [JsonKnownType(typeof(ParentClass), "parentclass")]
    [JsonKnownType(typeof(ParentClass2Heir), "parentclass2heir")]
    public class ParentClass
    {
        public string Summary { get; set; }
        public ChildClass2Heir Child { get; set; }
    }

    [JsonKnownThisType("parentclass1heir")]
    public class ParentClass1Heir : ParentClass
    {
        public int SomeInt { get; set; }
        public ChildClass1Heir Child2 { get; set; }
    }

    public class ParentClass2Heir : ParentClass
    {
        public double SomeDouble { get; set; }
        public string Detailed { get; set; }
        ChildClass Child2 { get; set; }
    }

    [JsonConverter(typeof(JsonKnownTypesConverter<ChildClass>))]
    [JsonKnownType(typeof(ChildClass), "childclass")]
    [JsonKnownType(typeof(ChildClass1Heir), "childclass1heir")]
    [JsonKnownType(typeof(ChildClass2Heir), "childclass2heir")]
    public class ChildClass
    {
        public string Summary { get; set; }
    }

    public class ChildClass1Heir : ChildClass
    {
        public int SomeInt { get; set; }
    }

    public class ChildClass2Heir : ChildClass
    {
        public double SomeDouble { get; set; }
        public string Detailed { get; set; }
    }
}
