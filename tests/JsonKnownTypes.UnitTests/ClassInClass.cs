using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    public class ClassInClass
    {
        [Test]
        public void BaseClassTest()
        {
            var c = new ClassWithClass
            {
                Name = "Test0",
                Children = new List<ClassWithClassBase>
                {
                    new ClassWithClassBase
                    {
                        Name = "name"
                    },
                    new ClassWithClass
                    {
                        Name = "name1",
                        Children = new List<ClassWithClassBase>
                        {
                            new ClassWithClassBase
                            {
                                Name = "name12"
                            }
                        }
                    }
                }
            };

            There_is_right_discriminator(c);
        }

        private void There_is_right_discriminator(ClassWithClass entity)
        {
            var json = JsonConvert.SerializeObject(entity);

            var obj = JsonConvert.DeserializeObject<ClassWithClass>(json);

            obj.Should().BeEquivalentTo(entity);
        }
    }

    [JsonConverter(typeof(JsonKnownTypesConverter<ClassWithClassBase>))]
    public class ClassWithClassBase
    {
        public string Name { get; set; }

    }

    public class ClassWithClass : ClassWithClassBase
    {
        public List<ClassWithClassBase> Children { get; set; } = new List<ClassWithClassBase>();
    }

}
