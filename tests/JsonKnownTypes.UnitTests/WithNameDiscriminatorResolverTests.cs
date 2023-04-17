using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    public class WithNameDiscriminatorResolverTests
    {
        [Test]
        public void BaseClassTest()
        {
            var entity = new a.ClassInContainer();
            JsonKnownTypesSettingsManager<ClassWithDuplicateChild>.DefaultDiscriminatorSettings.NameDiscriminatorResolver = type => type.FullName;
            var json = JsonConvert.SerializeObject(entity);

            json.Should().Be("{\"$type\":\"JsonKnownTypes.UnitTests.a.ClassInContainer\"}");

            var obj = JsonConvert.DeserializeObject<ClassWithDuplicateChild>(json);
            obj.GetType().Should().Be(entity.GetType());
        }
    }

    [JsonConverter(typeof(JsonKnownTypesConverter<ClassWithDuplicateChild>))]
    public class ClassWithDuplicateChild
    {
    }
}

namespace JsonKnownTypes.UnitTests.a
{

    public class ClassInContainer : ClassWithDuplicateChild
    {
    }
}

namespace JsonKnownTypes.UnitTests.b
{
    // ReSharper disable once UnusedType.Global - Justification: Used by deserializer as duplicate class example
    public class ClassInContainer : ClassWithDuplicateChild
    {
    }
}