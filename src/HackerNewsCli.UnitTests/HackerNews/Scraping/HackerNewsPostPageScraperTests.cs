using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using HackerNewsCli.HackerNews.Scraping;
using NUnit.Framework;

namespace HackerNewsCli.UnitTests.HackerNews.Scraping
{
    public class HackerNewsPostPageScraperTests
    {
        private static readonly Uri SourceUri = new Uri("https://news.ycombinator.com/?page=13");
        private MockMessageHandler _httpMessageHandler;
        private HackerNewsPostPageScraper _scraper;

        [SetUp]
        public void SetUp()
        {
            _httpMessageHandler = new MockMessageHandler();
            _scraper = new HackerNewsPostPageScraper(_httpMessageHandler);
        }

        private static IEnumerable<TestCaseData> SuccessTestCases()
        {
            yield return new TestCaseData(
                    ReadTestCase(nameof(SuccessTestCases), "single-external-link-post.html"),
                    new[]
                    {
                        new ScrapedPostContent(
                            SourceUri,
                            "Understanding the working of X11 from the ground up",
                            "https://magcius.github.io/xplain/article/index.html",
                            "signa11",
                            "116 points",
                            "14 comments",
                            "1."
                        )
                    })
                .SetName("GetPageModelAsync_SingleExternalLinkPost_ReturnsPostDataFromPage");

            yield return new TestCaseData(
                    ReadTestCase(nameof(SuccessTestCases), "single-discussion-post.html"),
                    new[]
                    {
                        new ScrapedPostContent(
                            SourceUri,
                            "Remind HN: Put contact info in your profile if you'd like to be contacted",
                            "item?id=21027730",
                            "whalesalad",
                            "171 points",
                            "73 comments",
                            "120."
                        )
                    })
                .SetName("GetPageModelAsync_SingleDiscussionPost_ReturnsPostDataFromPage");

            yield return new TestCaseData(
                    ReadTestCase(nameof(SuccessTestCases), "single-post-no-comments.html"),
                    new[]
                    {
                        new ScrapedPostContent(
                            SourceUri,
                            "The tech behind building an independent, internet radio station",
                            "https://watsonsmith.com.au/building-an-internet-radio-station",
                            "oggadog",
                            "9 points",
                            "discuss",
                            "4."
                        )
                    })
                .SetName("GetPageModelAsync_SinglePostNoComments_ReturnsPostDataFromPage");

            yield return new TestCaseData(
                    ReadTestCase(nameof(SuccessTestCases), "single-post-single-comment.html"),
                    new[]
                    {
                        new ScrapedPostContent(
                            SourceUri,
                            "Espresso: A Fast End-to-End Neural Speech Recognition Toolkit",
                            "https://arxiv.org/abs/1909.08723",
                            "sel1",
                            "46 points",
                            "1 comment",
                            "19."
                        )
                    })
                .SetName("GetPageModelAsync_SinglePostSingleComment_ReturnsPostDataFromPage");

            yield return new TestCaseData(
                    ReadTestCase(nameof(SuccessTestCases), "single-post-single-point.html"),
                    new[]
                    {
                        new ScrapedPostContent(
                            SourceUri,
                            "Which typeclass are you?",
                            "https://impurepics.com/quiz/",
                            "ptrkrlsrd",
                            "1 point",
                            "2 comments",
                            "1."
                        )
                    })
                .SetName("GetPageModelAsync_SinglePostSinglePoint_ReturnsPostDataFromPage");

            yield return new TestCaseData(
                    ReadTestCase(nameof(SuccessTestCases), "three-posts.html"),
                    new[]
                    {
                        new ScrapedPostContent(
                            SourceUri,
                            "Understanding the working of X11 from the ground up",
                            "https://magcius.github.io/xplain/article/index.html",
                            "signa11",
                            "162 points",
                            "32 comments",
                            "1."
                        ),
                        new ScrapedPostContent(
                            SourceUri,
                            "Brains are the last frontier of privacy",
                            "https://www.axios.com/robotic-brains-data-technology-companies-6ba7269e-1553-4395-a6db-3560fead7e24.html",
                            "hhs",
                            "88 points",
                            "49 comments",
                            "2."
                        ),
                        new ScrapedPostContent(
                            SourceUri,
                            "Postcode loophole enables fraudsters to hijack eBay parcels",
                            "https://www.theguardian.com/money/2019/sep/22/fraudsters-hijack-ebay-parcels-postcode-scam",
                            "jimnotgym",
                            "20 points",
                            "21 comments",
                            "3."
                        )
                    })
                .SetName("GetPageModelAsync_MultiplePosts_ReturnsPostDataFromPage");
        }

        private static IEnumerable<TestCaseData> FailureTestCases()
        {
            yield return new TestCaseData(
                    @"<html><head></head><body></body>")
                .SetName("GetPageModelAsync_NoPostsFound_ThrowsHackerNewsScrapeException");

            yield return new TestCaseData(
                    ReadTestCase(nameof(FailureTestCases), "single-post-missing-author.html"))
                .SetName("GetPageModelAsync_NoAuthorElement_ThrowsHackerNewsScrapeException");

            yield return new TestCaseData(
                    ReadTestCase(nameof(FailureTestCases), "single-post-missing-comments.html"))
                .SetName("GetPageModelAsync_NoCommentsElement_ThrowsHackerNewsScrapeException");

            yield return new TestCaseData(
                    ReadTestCase(nameof(FailureTestCases), "single-post-missing-rank.html"))
                .SetName("GetPageModelAsync_NoRankElement_ThrowsHackerNewsScrapeException");

            yield return new TestCaseData(
                    ReadTestCase(nameof(FailureTestCases), "single-post-missing-score.html"))
                .SetName("GetPageModelAsync_NoScoreElement_ThrowsHackerNewsScrapeException");

            yield return new TestCaseData(
                    ReadTestCase(nameof(FailureTestCases), "single-post-missing-storylink.html"))
                .SetName("GetPageModelAsync_NoStoryLinkElement_ThrowsHackerNewsScrapeException");
        }

        private static string ReadTestCase(string testCaseGroup, string resourceName)
        {
            var thisType = typeof(HackerNewsPostPageScraperTests);
            var testCase = thisType.Assembly.GetManifestResourceStream(thisType, $"{testCaseGroup}.{resourceName}");
            using (var reader = new StreamReader(testCase))
            {
                var testCaseContent = reader.ReadToEnd();
                return testCaseContent;
            }
        }

        [TestCaseSource(nameof(SuccessTestCases))]
        public async Task GetPostFromPageAsync_SuccessCases(string testHtml, ScrapedPostContent[] expectedPosts)
        {
            // Arrange
            _httpMessageHandler.SetupRequest(
                x => x.Method == HttpMethod.Get && x.RequestUri == SourceUri,
                response =>
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = new StringContent(testHtml, Encoding.UTF8, MediaTypeNames.Text.Html);
                });

            // Act
            var posts = await _scraper.GetPageModelAsync(SourceUri);

            // Assert
            Assert.That(posts.Length, Is.EqualTo(expectedPosts.Length));
            foreach (var postPair in posts.Zip(expectedPosts, (actual, expected) => new {ActualPost = actual, ExpectedPost = expected}))
            {
                Assert.That(postPair.ActualPost.TitleText, Is.EqualTo(postPair.ExpectedPost.TitleText));
                Assert.That(postPair.ActualPost.CommentsText, Is.EqualTo(postPair.ExpectedPost.CommentsText));
                Assert.That(postPair.ActualPost.AuthorText, Is.EqualTo(postPair.ExpectedPost.AuthorText));
                Assert.That(postPair.ActualPost.PointsText, Is.EqualTo(postPair.ExpectedPost.PointsText));
                Assert.That(postPair.ActualPost.RankText, Is.EqualTo(postPair.ExpectedPost.RankText));
                Assert.That(postPair.ActualPost.UriText, Is.EqualTo(postPair.ExpectedPost.UriText));
            }
        }

        [TestCaseSource(nameof(FailureTestCases))]
        public void GetPostFromPageAsync_FailureCases(string pageHtml)
        {
            // Arrange
            uint pageNumber = 13;
            var uri = new Uri($"https://news.ycombinator.com/?page={pageNumber}");
            _httpMessageHandler.SetupRequest(
                x => x.Method == HttpMethod.Get && x.RequestUri == uri,
                response =>
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = new StringContent(pageHtml, Encoding.UTF8, MediaTypeNames.Text.Html);
                });

            // Act + Assert
            Assert.ThrowsAsync<HackerNewsScrapeException>(async () => await _scraper.GetPageModelAsync(uri));
        }
    }
}