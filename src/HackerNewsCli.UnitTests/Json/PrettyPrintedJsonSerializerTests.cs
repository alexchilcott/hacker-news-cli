using System;
using HackerNewsCli.Json;
using NUnit.Framework;

namespace HackerNewsCli.UnitTests.Json
{
    public class PrettyPrintedJsonSerializerTests
    {
        private PrettyPrintedJsonSerializer _serializer;

        [SetUp]
        public void SetUp()
        {
            _serializer = new PrettyPrintedJsonSerializer();
        }

        [Test]
        public void Serialize_ReturnsIndentedAndCamelCasedJson()
        {
            // Arrange
            var input = new TestObject {DataValue = "Hello World"};

            // Act
            var json = _serializer.Serialize(input);

            // Assert
            var expectedJson = "{" + Environment.NewLine +
                               "  \"dataValue\": \"Hello World\"" + Environment.NewLine +
                               "}";
            Assert.That(json, Is.EqualTo(expectedJson));
        }

        private class TestObject
        {
            public string DataValue { get; set; }
        }
    }
}