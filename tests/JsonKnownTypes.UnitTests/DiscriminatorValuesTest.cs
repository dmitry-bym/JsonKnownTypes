using NUnit.Framework;

namespace JsonKnownTypes.UnitTests
{
    [TestFixture]
    public class DiscriminatorValuesTest
    {
        private record TestRecord(string FieldOne);
        private record TestHeir(string FieldOne, int FieldTwo) : TestRecord(FieldOne);
        private record TestFallback(string FieldOne, int FieldTwo) : TestRecord(FieldOne);

        [Test]
        public void CorrectDiscriminatorFieldNameAndBaseType()
        {
            var discriminatorFieldName = "discriminator";
            var expectedBaseType = typeof(TestRecord);
            
            var discriminatorValues = new DiscriminatorValues(expectedBaseType, discriminatorFieldName);

            Assert.AreEqual(discriminatorFieldName, discriminatorValues.FieldName);
            Assert.AreEqual(expectedBaseType, discriminatorValues.BaseType);
        }
        
        [Test]
        public void GetCorrectTypeAndDiscriminator()
        {   
            var discriminatorValues = new DiscriminatorValues(typeof(TestRecord), "name");

            discriminatorValues.AddType(typeof(TestHeir), nameof(TestHeir));

            var result = discriminatorValues.TryGetDiscriminator(typeof(TestHeir), out var disc);
            var result1 = discriminatorValues.TryGetType(nameof(TestHeir), out var type);
            
            Assert.True(result);
            Assert.True(result1);
            
            Assert.AreEqual(nameof(TestHeir), disc);
            Assert.AreEqual(typeof(TestHeir), type);
        }
        
        [Test]
        public void GetCorrectFallbackType()
        {
            var discriminatorValues = new DiscriminatorValues(typeof(TestRecord), "name");

            discriminatorValues.AddType(typeof(TestRecord), nameof(TestRecord));
            discriminatorValues.AddFallbackType(typeof(TestFallback));

            var result = discriminatorValues.TryGetType(nameof(TestHeir), out var type);
            
            Assert.True(result);
            
            Assert.AreEqual(typeof(TestFallback), type);
            Assert.AreEqual(typeof(TestFallback), discriminatorValues.FallbackType);
        }
        
        [Test]
        public void CorrectContains()
        {
            var discriminatorValues = new DiscriminatorValues(typeof(TestRecord), "name");

            discriminatorValues.AddType(typeof(TestRecord), nameof(TestRecord));

            var result = discriminatorValues.Contains(nameof(TestHeir));
            
            Assert.True(result);
        }
    }
}
