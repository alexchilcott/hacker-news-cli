using System;
using HackerNewsCli.HackerNews.Scraping;
using NUnit.Framework;

namespace HackerNewsCli.UnitTests.HackerNews.Scraping
{
    public class ScrapedPostContentParserTests
    {
        private const string VALID_AUTHOR = "someauthor";
        private const string VALID_TITLE = "A valid title for a post";
        private const string VALID_ABSOLUTE_URI = "http://www.bbc.co.uk/news";
        private const string VALID_RELATIVE_URI = "news/1234";
        private const string VALID_POINTS_TEXT = "38 points";
        private const string VALID_COMMENTS_TEXT = "12 comments";
        private const string VALID_RANK_TEXT = "7.";

        private static readonly Uri ValidBaseUri = new Uri("http://www.bbc.co.uk");
        private readonly string _stringWith256Characters = "".PadRight(256, 'a');
        private readonly string _stringWith257Characters = "".PadRight(257, 'a');

        private ScrapedPostContentParser _contentParser;

        [SetUp]
        public void SetUp()
        {
            _contentParser = new ScrapedPostContentParser();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Convert_AuthorNullOrWhitespace_ThrowsHackerNewsScrapeException(string authorText)
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                VALID_ABSOLUTE_URI,
                authorText,
                VALID_POINTS_TEXT,
                VALID_COMMENTS_TEXT,
                VALID_RANK_TEXT);

            // Act + Assert
            Assert.Throws<HackerNewsScrapeException>(() => _contentParser.Convert(rawContent));
        }

        [Test]
        public void Convert_AuthorTooLong_ThrowsHackerNewsScrapeException()
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                VALID_ABSOLUTE_URI,
                _stringWith257Characters,
                VALID_POINTS_TEXT,
                VALID_COMMENTS_TEXT,
                VALID_RANK_TEXT);

            // Act + Assert
            Assert.Throws<HackerNewsScrapeException>(() => _contentParser.Convert(rawContent));
        }

        [Test]
        public void Convert_ValidRequest_ReturnsPostWithCorrectAuthor()
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                VALID_ABSOLUTE_URI,
                _stringWith256Characters,
                VALID_POINTS_TEXT,
                VALID_COMMENTS_TEXT,
                VALID_RANK_TEXT);

            // Act
            var post = _contentParser.Convert(rawContent);

            // Assert
            Assert.That(post.Author, Is.EqualTo(_stringWith256Characters));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Convert_TitleNullOrWhitespace_ThrowsHackerNewsScrapeException(string titleText)
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                titleText,
                VALID_ABSOLUTE_URI,
                VALID_AUTHOR,
                VALID_POINTS_TEXT,
                VALID_COMMENTS_TEXT,
                VALID_RANK_TEXT);

            // Act + Assert
            Assert.Throws<HackerNewsScrapeException>(() => _contentParser.Convert(rawContent));
        }

        [Test]
        public void Convert_TitleTooLong_ThrowsHackerNewsScrapeException()
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                _stringWith257Characters,
                VALID_ABSOLUTE_URI,
                VALID_AUTHOR,
                VALID_POINTS_TEXT,
                VALID_COMMENTS_TEXT,
                VALID_RANK_TEXT);

            // Act + Assert
            Assert.Throws<HackerNewsScrapeException>(() => _contentParser.Convert(rawContent));
        }

        [Test]
        public void Convert_ValidRequest_ReturnsPostWithCorrectTitle()
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                _stringWith256Characters,
                VALID_ABSOLUTE_URI,
                VALID_AUTHOR,
                VALID_POINTS_TEXT,
                VALID_COMMENTS_TEXT,
                VALID_RANK_TEXT);

            // Act
            var post = _contentParser.Convert(rawContent);

            // Assert
            Assert.That(post.Title, Is.EqualTo(_stringWith256Characters));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Convert_UriNullOrWhiteSpace_ThrowsHackerNewsScrapeException(string uri)
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                uri,
                VALID_AUTHOR,
                VALID_POINTS_TEXT,
                VALID_COMMENTS_TEXT,
                VALID_RANK_TEXT);

            // Act + Assert
            Assert.Throws<HackerNewsScrapeException>(() => _contentParser.Convert(rawContent));
        }

        [Test]
        public void Convert_ValidAbsoluteUri_ReturnsPostWithSameUri()
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                VALID_ABSOLUTE_URI,
                VALID_AUTHOR,
                VALID_POINTS_TEXT,
                VALID_COMMENTS_TEXT,
                VALID_RANK_TEXT);

            // Act
            var parsedPostModel = _contentParser.Convert(rawContent);

            // Assert
            Assert.That(parsedPostModel.Uri, Is.EqualTo(new Uri(VALID_ABSOLUTE_URI)));
        }

        [Test]
        public void Convert_ValidRelativeUri_ReturnsPostWithAbsoluteUri()
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                VALID_RELATIVE_URI,
                VALID_AUTHOR,
                VALID_POINTS_TEXT,
                VALID_COMMENTS_TEXT,
                VALID_RANK_TEXT);

            // Act
            var parsedPostModel = _contentParser.Convert(rawContent);

            // Assert
            Assert.That(parsedPostModel.Uri, Is.EqualTo(new Uri(ValidBaseUri, VALID_RELATIVE_URI)));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("-1 points")]
        public void Convert_PointsNullOrWhiteSpaceOrNegative_ThrowsHackerNewsScrapeException(string pointsText)
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                VALID_ABSOLUTE_URI,
                VALID_AUTHOR,
                pointsText,
                VALID_COMMENTS_TEXT,
                VALID_RANK_TEXT);

            // Act + Assert
            Assert.Throws<HackerNewsScrapeException>(() => _contentParser.Convert(rawContent));
        }

        [TestCase("0 points", 0)]
        [TestCase("1 point", 1)]
        [TestCase("2 points", 2)]
        public void Convert_ValidPointsText_ReturnsPostWithCorrectPoints(string pointsText, int expectedPoints)
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                VALID_ABSOLUTE_URI,
                VALID_AUTHOR,
                pointsText,
                VALID_COMMENTS_TEXT,
                VALID_RANK_TEXT);

            // Act
            var parsedPostModel = _contentParser.Convert(rawContent);

            // Assert
            Assert.That(parsedPostModel.Points, Is.EqualTo(expectedPoints));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("-1 comments")]
        public void Convert_CommentsNullOrWhiteSpaceOrNegative_ThrowsHackerNewsScrapeException(string commentsText)
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                VALID_ABSOLUTE_URI,
                VALID_AUTHOR,
                VALID_POINTS_TEXT,
                commentsText,
                VALID_RANK_TEXT);

            // Act + Assert
            Assert.Throws<HackerNewsScrapeException>(() => _contentParser.Convert(rawContent));
        }

        [TestCase("discuss", 0)]
        [TestCase("1 comment", 1)]
        [TestCase("2 comments", 2)]
        public void Convert_ValidCommentsText_ReturnsPostWithCorrectComments(string commentsText, int expectedComments)
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                VALID_ABSOLUTE_URI,
                VALID_AUTHOR,
                VALID_POINTS_TEXT,
                commentsText,
                VALID_RANK_TEXT);

            // Act
            var parsedPostModel = _contentParser.Convert(rawContent);

            // Assert
            Assert.That(parsedPostModel.Comments, Is.EqualTo(expectedComments));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Convert_RankNullOrWhiteSpace_ThrowsHackerNewsScrapeException(string rankText)
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                VALID_ABSOLUTE_URI,
                VALID_AUTHOR,
                VALID_POINTS_TEXT,
                VALID_COMMENTS_TEXT,
                rankText);

            // Act + Assert
            Assert.Throws<HackerNewsScrapeException>(() => _contentParser.Convert(rawContent));
        }

        [TestCase("0.")]
        [TestCase("-1.")]
        public void Convert_RankNotStrictlyPositive_ThrowsHackerNewsScrapeException(string rankText)
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                VALID_ABSOLUTE_URI,
                VALID_AUTHOR,
                VALID_POINTS_TEXT,
                VALID_COMMENTS_TEXT,
                rankText);

            // Act + Assert
            Assert.Throws<HackerNewsScrapeException>(() => _contentParser.Convert(rawContent));
        }

        [TestCase("1.", 1)]
        [TestCase("10.", 10)]
        public void Convert_ValidRankText_ReturnsPostWithCorrectRank(string rankText, int expectedRank)
        {
            // Arrange
            var rawContent = new ScrapedPostContent(
                ValidBaseUri,
                VALID_TITLE,
                VALID_ABSOLUTE_URI,
                VALID_AUTHOR,
                VALID_POINTS_TEXT,
                VALID_COMMENTS_TEXT,
                rankText);

            // Act
            var parsedPostModel = _contentParser.Convert(rawContent);

            // Assert
            Assert.That(parsedPostModel.Rank, Is.EqualTo(expectedRank));
        }
    }
}