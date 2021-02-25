using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonKnownTypes.UnitTests.A
{

    [JsonKnownThisType]
    public class ImplementingClass : IValue
    {
        public string Value { get; set; }
    }
}

namespace JsonKnownTypes.UnitTests.B
{

    [JsonKnownThisType]
    public class ImplementingClass : A.ImplementingClass { }
}

namespace JsonKnownTypes.UnitTests
{
    public interface IValue
    {
        string Value { get; }
    }

    public class Container
    {
        [JsonConverter(typeof(JsonKnownTypesConverter<IValue>))]
        public IValue Value { get; set; }
    }

    public class DerivedTypesWithSameDiscriminatorValue
    {
        [Test]
        public void ShouldBeAbleToDeserializeDerivedClassAsBaseClass()
        {
            JsonKnownTypesSettingsManager.GetDerivedByBase = parent => new[] { typeof(A.ImplementingClass) };

            var container = new Container { Value = new B.ImplementingClass { Value = "Blah!" } };

            var serialized = JsonConvert.SerializeObject(container);

            JsonKnownTypesSettingsManager.GetDerivedByBase = parent => new[] { typeof(A.ImplementingClass) };

            var actual = JsonConvert.DeserializeObject<Container>(serialized);

            Assert.That(actual.Value, Is.InstanceOf(typeof(A.ImplementingClass)));
            Assert.That(actual.Value.Value, Is.EqualTo("Blah!"));

            // Reset
            JsonKnownTypesSettingsManager.GetDerivedByBase = parent => parent.Assembly.GetTypes();
        }
    }
}
