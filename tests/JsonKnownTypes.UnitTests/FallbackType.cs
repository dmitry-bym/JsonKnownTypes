using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    [JsonConverter(typeof(JsonKnownTypesConverter<OperationBase>))]
    [JsonDiscriminator(Name = "opType")]
    [JsonKnownType(typeof(OperationStarted), "op_start")]
    [JsonKnownType(typeof(OperationEnded), "op_end")]
    [JsonKnownTypeFallback(typeof(UnknownOperation))]
    public abstract class OperationBase
    {
        public string Id { get; set; }
    }

    public class OperationStarted : OperationBase { }

    public class OperationEnded : OperationBase { }

    public class UnknownOperation : OperationBase { }

    public class FallbackType
    {
        [Test]
        public void Deserialize_UsesFallbackType_WhenUnknownDiscriminatorIsUsed()
        {
            const string payload = @"[
{id: ""abc"", opType: ""op_start""},
{id: ""bcd"", opType: ""op_delayed""},
{id: ""cde"", opType: ""op_update""},
{id: ""def"", opType: ""op_end""},
{id: ""broken_entity""}
]";

            var deserialized = JsonConvert.DeserializeObject<OperationBase[]>(payload);

            Assert.AreEqual(typeof(OperationStarted), deserialized[0].GetType());
            Assert.AreEqual(typeof(UnknownOperation), deserialized[1].GetType());
            Assert.AreEqual(typeof(UnknownOperation), deserialized[2].GetType());
            Assert.AreEqual(typeof(OperationEnded), deserialized[3].GetType());
            Assert.AreEqual(typeof(UnknownOperation), deserialized[4].GetType());
        }

        [Test]
        public void Serialize_and_Deserialize_ProducesSameResult()
        {
            var serialized = JsonConvert.SerializeObject(new OperationBase[]
            {
                new OperationStarted { Id = "abc" },
                new UnknownOperation { Id = "bcd" }
            });

            var deserialized = JsonConvert.DeserializeObject<OperationBase[]>(serialized);

            Assert.AreEqual(typeof(UnknownOperation), deserialized[1].GetType());
        }
    }
}
