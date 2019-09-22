using System;
using System.Linq;
using System.Threading.Tasks;
using HackerNewsCli.Scraping;
using Moq;
using NUnit.Framework;

namespace HackerNewsCli.UnitTests.Scraping
{
    public class ConvertingScraperTests
    {
        [Test]
        public async Task GetPageModel_ConvertsAndReturnsModelFromUnderlyingScraper()
        {
            // Arrange
            var testUri = new Uri("http://www.bbc.co.uk/");
            var unconvertedPageModel = "olleh"; // represents an 'intermediate' form of the page being scraped
            var mockPageScraper = new Mock<IScraper<string>>();
            mockPageScraper.Setup(x => x.GetPageModelAsync(testUri)).ReturnsAsync(unconvertedPageModel);
            var reversingConverter = new DelegateConverter<string, string>(ReverseString);
            var convertingScraper = new ConvertingScraper<string, string>(mockPageScraper.Object, reversingConverter);

            // Act
            var convertedPageModel = await convertingScraper.GetPageModelAsync(testUri);

            // Assert
            var expectedConvertedPageModel = "hello";
            Assert.That(convertedPageModel, Is.EqualTo(expectedConvertedPageModel));
        }

        private string ReverseString(string arg)
        {
            return new string(arg.Reverse().ToArray());
        }
    }
}