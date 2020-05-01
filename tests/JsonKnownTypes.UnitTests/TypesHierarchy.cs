using AutoFixture.NUnit3;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    [TestFixture]
    public class TypesHierarchy
    {
        [Test]
        [AutoData]
        public void BaseAbstractClass1HeirWithBaseClass(BaseAbstractClass1Heir entity, BaseClass field)
        {
            entity.Field = field;
            There_is_right_discriminator(entity);
        }

        [Test]
        [AutoData]
        public void BaseAbstractClass1HeirWithBaseClass1Heir(BaseAbstractClass1Heir entity, BaseClass1Heir field)
        {
            entity.Field = field;
            There_is_right_discriminator(entity);
        }

        [Test]
        [AutoData]
        public void BaseAbstractClass1HeirWithBaseClass2Heir(BaseAbstractClass1Heir entity, BaseClass2Heir field)
        {
            entity.Field = field;
            There_is_right_discriminator(entity);
        }

        [Test]
        [AutoData]
        public void BaseAbstractClass1HeirWithBaseClass3Heir(BaseAbstractClass1Heir entity, BaseClass3Heir field)
        {
            entity.Field = field;
            There_is_right_discriminator(entity);
        }

        [Test]
        [AutoData]
        public void BaseAbstractClass2HeirWithBaseClass(BaseAbstractClass2Heir entity, BaseClass field)
        {
            entity.Field = field;
            There_is_right_discriminator(entity);
        }

        [Test]
        [AutoData]
        public void BaseAbstractClass2HeirWithBaseClass1Heir(BaseAbstractClass2Heir entity, BaseClass1Heir field)
        {
            entity.Field = field;
            There_is_right_discriminator(entity);
        }

        [Test]
        [AutoData]
        public void BaseAbstractClass2HeirWithBaseClass2Heir(BaseAbstractClass2Heir entity, BaseClass2Heir field)
        {
            entity.Field = field;
            There_is_right_discriminator(entity);
        }

        [Test]
        [AutoData]
        public void BaseAbstractClass2HeirWithBaseClass3Heir(BaseAbstractClass2Heir entity, BaseClass3Heir field)
        {
            entity.Field = field;
            There_is_right_discriminator(entity);
        }

        private void There_is_right_discriminator(BaseAbstractClass entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var jObject = JToken.Parse(json);

            var discriminator = jObject["$type"]?.Value<string>();
            var obj = JsonConvert.DeserializeObject<BaseAbstractClass>(json);

            obj.Should().BeEquivalentTo(entity);
            Assert.AreEqual(discriminator, entity.GetType().Name);
        }

        [JsonConverter(typeof(JsonKnownTypesConverter<BaseClass>))]
        [JsonDiscriminator(UseClassNameAsDiscriminator = false)]
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

        [JsonConverter(typeof(JsonKnownTypesConverter<BaseAbstractClass>))]
        [JsonKnownType(typeof(BaseAbstractClass1Heir))]
        [JsonKnownType(typeof(BaseAbstractClass2Heir))]
        public abstract class BaseAbstractClass
        {
            public string Summary { get; set; }

            public BaseClass Field { get; set; }
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
}
