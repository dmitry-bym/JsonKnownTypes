using System;
using AutoFixture.NUnit3;
using JsonKnownTypes.Exceptions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    [TestFixture]
    class WithBaseClassWithException
    {
        private static string DiscriminatorName { get => "$type"; }

        [Test]
        [AutoData]
        public void AutoBaseClassSameDiscriminator(BaseClassSameDiscriminator entity)
            => Should_throw_exception(entity);

        [Test]
        [AutoData]
        public void AutoBaseClassSameDiscriminator1Heir(BaseClassSameDiscriminator1Heir entity)
            => Should_throw_exception(entity);

        [Test]
        [AutoData]
        public void AutoBaseClassSameDiscriminator2Heir(BaseClassSameDiscriminator2Heir entity)
            => Should_throw_exception(entity);

        [Test]
        [AutoData]
        public void AutoBaseClassSameDiscriminator3Heir(BaseClassSameDiscriminator3Heir entity)
            => Should_throw_exception(entity);

        private void Should_throw_exception(BaseClassSameDiscriminator entity)
        {
            Assert.Throws<JsonException>(delegate
            {
                JsonConvert.SerializeObject(entity);
            });
        }
    }

    [JsonConverter(typeof(JsonKnownTypesConverter<BaseClassSameDiscriminator>))]
    [JsonKnownType(typeof(BaseClassSameDiscriminator))]
    [JsonKnownType(typeof(BaseClassSameDiscriminator1Heir), "same name")]
    [JsonKnownType(typeof(BaseClassSameDiscriminator2Heir), "same name")]
    [JsonKnownType(typeof(BaseClassSameDiscriminator3Heir), "same name")]
    public class BaseClassSameDiscriminator
    {
        public string Summary { get; set; }
    }

    public class BaseClassSameDiscriminator1Heir : BaseClassSameDiscriminator
    {
        public int SomeInt { get; set; }
    }

    public class BaseClassSameDiscriminator2Heir : BaseClassSameDiscriminator1Heir
    {
        public double SomeDouble { get; set; }
        public string Detailed { get; set; }
    }

    public class BaseClassSameDiscriminator3Heir : BaseClassSameDiscriminator2Heir
    {
        public string SomeString { get; set; }
    }
}
