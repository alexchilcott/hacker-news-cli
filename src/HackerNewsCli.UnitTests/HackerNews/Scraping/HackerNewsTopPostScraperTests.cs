using System;
using System.Linq;
using System.Threading.Tasks;
using HackerNewsCli.HackerNews;
using HackerNewsCli.HackerNews.Scraping;
using HackerNewsCli.Scraping;
using Moq;
using NUnit.Framework;

namespace HackerNewsCli.UnitTests.HackerNews.Scraping
{
    public class HackerNewsTopPostScraperTests
    {
        private readonly Random _random = new Random();
        private Mock<IScraper<Post[]>> _mockScraper;
        private HackerNewsTopPostScraper _scraper;

        [SetUp]
        public void SetUp()
        {
            _mockScraper = new Mock<IScraper<Post[]>>();
            _scraper = new HackerNewsTopPostScraper(_mockScraper.Object);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void GetTopPostsAsync_NumberOfPostsLessThan0_ThrowsArgumentException(int numPosts)
        {
            // Act + Assert
            Assert.ThrowsAsync<ArgumentException>(() => _scraper.GetTopPostsAsync(numPosts));
        }

        [Test]
        public async Task GetTopPostsAsync_AllPostsAvailable_ReturnsPosts()
        {
            // Arrange
            var expectedUriPrefix = "https://news.ycombinator.com/news?p=";
            var postsOnPage1 = CreatePosts(10, 1);
            var postsOnPage2 = CreatePosts(10, 11);
            var postsOnPage3 = CreatePosts(10, 21);
            _mockScraper.Setup(x => x.GetPageModelAsync(new Uri(expectedUriPrefix + "1")))
                .Returns(Task.FromResult(postsOnPage1));
            _mockScraper.Setup(x => x.GetPageModelAsync(new Uri(expectedUriPrefix + "2")))
                .Returns(Task.FromResult(postsOnPage2));
            _mockScraper.Setup(x => x.GetPageModelAsync(new Uri(expectedUriPrefix + "3")))
                .Returns(Task.FromResult(postsOnPage3));
            var numberToRetrieve = 25;

            // Act
            var results = await _scraper.GetTopPostsAsync(numberToRetrieve);

            // Assert
            var expectedResults = postsOnPage1.Concat(postsOnPage2).Concat(postsOnPage3).Take(numberToRetrieve).ToArray();
            AssertPostArraysAreEquivalent(results, expectedResults);
        }

        [Test]
        public async Task GetTopPostsAsync_OnlySomePostsAvailable_ReturnsAllAvailablePosts()
        {
            // Arrange
            var expectedUriPrefix = "https://news.ycombinator.com/news?p=";
            var postsOnPage1 = CreatePosts(10, 1);
            var postsOnPage2 = CreatePosts(5, 11);

            _mockScraper.Setup(x => x.GetPageModelAsync(new Uri(expectedUriPrefix + "1")))
                .Returns(Task.FromResult(postsOnPage1));
            _mockScraper.Setup(x => x.GetPageModelAsync(new Uri(expectedUriPrefix + "2")))
                .Returns(Task.FromResult(postsOnPage2));
            var numberToRetrieve = 25;

            // Act
            var results = await _scraper.GetTopPostsAsync(numberToRetrieve);

            // Assert
            var expectedResults = postsOnPage1.Concat(postsOnPage2).ToArray();
            AssertPostArraysAreEquivalent(results, expectedResults);
        }

        private void AssertPostArraysAreEquivalent(Post[] actualPosts, Post[] expectedPosts)
        {
            Assert.That(actualPosts.Length, Is.EqualTo(expectedPosts.Length));
            foreach (var pair in actualPosts.Zip(expectedPosts, (actual, expected) => new {ActualPost = actual, ExpectedPost = expected}))
            {
                Assert.That(pair.ActualPost.Uri, Is.EqualTo(pair.ExpectedPost.Uri));
                Assert.That(pair.ActualPost.Author, Is.EqualTo(pair.ExpectedPost.Author));
                Assert.That(pair.ActualPost.Comments, Is.EqualTo(pair.ExpectedPost.Comments));
                Assert.That(pair.ActualPost.Points, Is.EqualTo(pair.ExpectedPost.Points));
                Assert.That(pair.ActualPost.Rank, Is.EqualTo(pair.ExpectedPost.Rank));
                Assert.That(pair.ActualPost.Title, Is.EqualTo(pair.ExpectedPost.Title));
            }
        }

        private Post[] CreatePosts(int numberOfPosts, int firstRank)
        {
            return Enumerable.Range(firstRank, numberOfPosts).Select(CreatePost).ToArray();
        }

        private Post CreatePost(int rank)
        {
            return new Post(
                $"title-{Guid.NewGuid()}",
                new Uri($"http://www.bbc.co.uk/story/{Guid.NewGuid()}"),
                $"author-{Guid.NewGuid()}",
                _random.Next(0, 100),
                _random.Next(0, 100),
                rank);
        }
    }
}