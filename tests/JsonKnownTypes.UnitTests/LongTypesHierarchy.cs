using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    [TestFixture]
    public class LongTypesHierarchy
    {
        public class AutoDataCustomAttribute : AutoDataAttribute
        {
            public AutoDataCustomAttribute()
                : base(() =>
                {
                    var fixture = new Fixture();
                    fixture.Register<BaseAbstractClass>(() => null);
                    fixture.Register<IBaseInterface>(() => null);
                    return fixture;
                })
            { }
        }

        [Test]
        [AutoDataCustom]
        public void BaseClassTest(BaseClass entity)
        {
            There_is_right_discriminator(entity);
        }

        [Test]
        [AutoDataCustom]
        public void BaseClass1HeirWithBaseAbstractClass1Heir(BaseClass1Heir entity, BaseAbstractClass1Heir field)
        {
            entity.Field = new ClassWithoutHeirs { Field = field };
            There_is_right_discriminator(entity);
        }

        [Test]
        [AutoDataCustom]
        public void BaseClass1HeirWithBaseAbstractClass2Heir(BaseClass1Heir entity, BaseAbstractClass2Heir field)
        {
            entity.Field = new ClassWithoutHeirs { Field = field };
            There_is_right_discriminator(entity);
        }

        [Test]
        [AutoData]
        public void BaseClass2HeirTest(BaseClass2Heir entity)
        {
            There_is_right_discriminator(entity);
        }

        [Test]
        [AutoDataCustom]
        public void BaseClass3HeirWithBaseInterface1Heir(BaseClass3Heir entity, BaseInterface1Heir field)
        {
            entity.Field = new ClassWithoutHeirs2 { Field = field };
            There_is_right_discriminator(entity);
        }

        [Test]
        [AutoDataCustom]
        public void BaseClass3HeirWithBaseInterface2Heir(BaseClass3Heir entity, BaseInterface2Heir field)
        {
            entity.Field = new ClassWithoutHeirs2 { Field = field };
            There_is_right_discriminator(entity);
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
        [JsonDiscriminator(AddAutoDiscriminators = false)]
        [JsonKnownThisType]
        public class BaseClass
        {
            public string Summary { get; set; }
        }

        [JsonKnownThisType]
        public class BaseClass1Heir : BaseClass
        {
            public int SomeInt { get; set; }
            public ClassWithoutHeirs Field { get; set; }
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
            public ClassWithoutHeirs2 Field { get; set; }
        }

        public class ClassWithoutHeirs
        {
            public BaseAbstractClass Field;
        }

        [JsonConverter(typeof(JsonKnownTypesConverter<BaseAbstractClass>))]
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

        public class ClassWithoutHeirs2
        {
            public IBaseInterface Field { get; set; }
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
}
