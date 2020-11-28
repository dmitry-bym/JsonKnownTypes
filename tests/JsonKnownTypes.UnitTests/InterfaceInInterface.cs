using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    public class InterfaceInInterface
    {
        [Test]
        public void BaseClassTest()
        {
            var holder = new ObjectHolder();

            var twc = new TestWithChildren { Name = "Test0" };
            twc.Children.Add(new TestWithDataOnly { Name = "Test1", Value = 1 });
            twc.Children.Add(new TestWithDataOnly { Name = "Test2", Value = 2 });

            holder.Tests.Add(twc);
            holder.Tests.Add(new TestWithDataOnly { Name = "Test3", Value = 3 });
            holder.Tests.Add(new TestWithDataOnly { Name = "Test4", Value = 4 });
            holder.Tests.Add(new TestWithDataOnly { Name = "Test5", Value = 5 });

            There_is_right_discriminator(holder);
        }

        private void There_is_right_discriminator(ObjectHolder entity)
        {
            var json = JsonConvert.SerializeObject(entity);

            var obj = JsonConvert.DeserializeObject<ObjectHolder>(json);

            obj.Should().BeEquivalentTo(entity);
        }
    }

    [JsonConverter(typeof(JsonKnownTypesConverter<ITest>))]
    public interface ITest
    { }

    public class TestWithChildren : ITest
    {
        public string Name { get; set; }
        public List<ITest> Children { get; set; } = new List<ITest>();
    }

    public class TestWithDataOnly : ITest
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class ObjectHolder
    {
        public List<ITest> Tests { get; protected set; } = new List<ITest>();
    }
}
