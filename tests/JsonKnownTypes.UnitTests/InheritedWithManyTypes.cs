using System;
using System.Collections;
using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    public class InheritedWithManyTypes
    {
        [JsonConverter(typeof(JsonKnownTypesConverter<TestBase>))]
        private record TestBase(string Str);

        private record TestInherited(string Str,
            DateTime DateTimeField,
            DateTimeOffset DateTimeOffsetField,
            decimal DecimalField,
            double DoubleField,
            bool BoolField,
            int IntField,
            byte ByteField,
            byte[] BytesArrayField) : TestBase(Str);

        [Test]
        public void CorrectSerializationAndDeserialization()
        {
            There_is_right_discriminator(new Fixture().Create<TestInherited>());
        }

        private void There_is_right_discriminator(TestBase entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var jObject = JToken.Parse(json);

            var discriminator = jObject["$type"]?.Value<string>();
            var obj = JsonConvert.DeserializeObject<TestBase>(json);

            var ex = (TestInherited) entity;
            var ac = (TestInherited) obj;

            Assert.AreEqual(discriminator, entity.GetType().Name);
            
            Assert.AreEqual(ex.Str, ac.Str);
            Assert.AreEqual(ex.DateTimeField, ac.DateTimeField);
            Assert.AreEqual(ex.DateTimeOffsetField, ac.DateTimeOffsetField);
            Assert.AreEqual(ex.DecimalField, ac.DecimalField);
            Assert.AreEqual(ex.DoubleField, ac.DoubleField);
            Assert.AreEqual(ex.BoolField, ac.BoolField);
            Assert.AreEqual(ex.IntField, ac.IntField);
            Assert.AreEqual(ex.ByteField, ac.ByteField);
            for (int i = 0; i < ex.BytesArrayField.Length; i++)
            {
                Assert.AreEqual(ex.BytesArrayField[i], ac.BytesArrayField[i]);
            }
        }
    }
}
