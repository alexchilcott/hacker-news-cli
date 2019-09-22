using HackerNewsCli.Scraping;
using NUnit.Framework;

namespace HackerNewsCli.UnitTests.Scraping
{
    public class ArrayConverterTests
    {
        [Test]
        public void Convert_ConvertsArrayUsingUnderlyingConverterForEachItem()
        {
            // Arrange
            var arrayConverter = new ArrayConverter<string, int>(new DelegateConverter<string, int>(int.Parse));
            string[] strings = {"1", "2", "3"};

            // Act
            var convertedInts = arrayConverter.Convert(strings);

            // Assert
            Assert.That(convertedInts, Is.EquivalentTo(new[] {1, 2, 3}));
        }
    }
}