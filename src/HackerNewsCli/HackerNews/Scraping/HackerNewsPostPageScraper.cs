using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using HackerNewsCli.Scraping;

namespace HackerNewsCli.HackerNews.Scraping
{
    /// <summary>
    ///     Scrapes the hacker news post pages and returns an array of <see cref="ScrapedPostContent" />s, with each item
    ///     representing a post.
    ///     <see cref="ScrapedPostContent" /> objects contain the raw, un-parsed text of the posts. The focus of this
    ///     <see cref="IScraper{T}" /> is to read the raw text from HTML elements. The parsing of this data occurs in
    ///     <see cref="ScrapedPostContentParser" />.
    /// </summary>
    public class HackerNewsPostPageScraper : IScraper<ScrapedPostContent[]>
    {
        private readonly HttpMessageHandler _httpMessageHandler;

        public HackerNewsPostPageScraper(HttpMessageHandler httpMessageHandler)
        {
            _httpMessageHandler = httpMessageHandler;
        }

        public async Task<ScrapedPostContent[]> GetPageModelAsync(Uri pageUri)
        {
            var config = Configuration.Default
                .WithRequesters(_httpMessageHandler)
                .WithDefaultLoader();

            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(pageUri.ToString());
            var postTitleTableRowCellSelector = ".itemlist tr.athing";
            var postTitleTableRowCells = document.QuerySelectorAll(postTitleTableRowCellSelector);

            if (!postTitleTableRowCells.Any())
            {
                throw new HackerNewsScrapeException($"Could not find any title row cells on page at {pageUri}");
            }

            return postTitleTableRowCells
                .Select(x => ScrapePostFromPage(x, pageUri))
                .Where(x => x != null)
                .ToArray();
        }

        private ScrapedPostContent ScrapePostFromPage(IElement postTitleTableRowCell, Uri pageUri)
        {
            var postSubRowTableRowCell = postTitleTableRowCell.NextElementSibling;

            // Get the raw text from various elements in the DOM.
            var rankElementText = ScrapeRankElementText(postTitleTableRowCell, pageUri);
            var titleElementText = ScrapeTitleElementText(postTitleTableRowCell, pageUri);
            var uriElementText = ScrapeLinkHrefText(postTitleTableRowCell, pageUri);
            var pointsElementText = ScrapePointsElementText(postSubRowTableRowCell, pageUri);
            var authorElementText = ScrapeAuthorElementText(postSubRowTableRowCell, pageUri);
            var commentsElementText = ScrapeCommentsElementText(postSubRowTableRowCell, pageUri);
            if (rankElementText == null
                || titleElementText == null
                || uriElementText == null
                || pointsElementText == null
                || authorElementText == null
                || commentsElementText == null)
            {
                return null;
            }

            var scrapeResult = new ScrapedPostContent(
                pageUri,
                titleElementText,
                uriElementText,
                authorElementText,
                pointsElementText,
                commentsElementText,
                rankElementText);
            return scrapeResult;
        }

        private string ScrapeCommentsElementText(IElement postSubRowTableRowCell, Uri pageUri)
        {
            var commentsElement = postSubRowTableRowCell.QuerySelectorAll("a[href]").Last();
            if (commentsElement == null)
            {
                return null;
            }

            var commentsElementText = commentsElement.TextContent;
            return SanitizeText(commentsElementText);
        }

        private string ScrapeAuthorElementText(IElement postSubRowTableRowCell, Uri pageUri)
        {
            var authorElement = postSubRowTableRowCell.QuerySelector("a.hnuser");
            if (authorElement == null)
            {
                return null;
            }

            var author = authorElement.TextContent;
            return SanitizeText(author);
        }

        private string ScrapePointsElementText(IElement postSubRowTableRowCell, Uri pageUri)
        {
            var pointsElementForPost = postSubRowTableRowCell.QuerySelector(".score");
            if (pointsElementForPost == null)
            {
                return null;
            }

            var pointsElementText = pointsElementForPost.TextContent;
            return SanitizeText(pointsElementText);
        }

        private string ScrapeLinkHrefText(IElement postTitleTableRowCell, Uri pageUri)
        {
            var postLinkElement = GetPostLinkElement(postTitleTableRowCell, pageUri);
            var absoluteOrRelativeUri = postLinkElement.GetAttribute("href");
            return absoluteOrRelativeUri;
        }

        private string ScrapeTitleElementText(IElement postTitleTableRowCell, Uri pageUri)
        {
            var postLinkElement = GetPostLinkElement(postTitleTableRowCell, pageUri);
            var title = postLinkElement.TextContent;
            return SanitizeText(title);
        }

        private static IElement GetPostLinkElement(IElement postTitleTableRowCell, Uri pageUri)
        {
            var postLinkElement = postTitleTableRowCell.QuerySelector("td.title a[href].storylink");
            if (postLinkElement == null)
            {
                throw new HackerNewsScrapeException($"No post link element could be found for a post on {pageUri}");
            }

            return postLinkElement;
        }

        private string ScrapeRankElementText(IElement postTitleTableRowCell, Uri pageUri)
        {
            var rankElementForPost = postTitleTableRowCell.QuerySelector(".rank");
            if (rankElementForPost == null)
            {
                throw new HackerNewsScrapeException($"No rank element could be determined for a post on {pageUri}");
            }

            var rankElementText = SanitizeText(rankElementForPost.TextContent);
            return rankElementText;
        }

        private string SanitizeText(string textContent)
        {
            return textContent
                .Trim()
                .Replace('\u00A0', ' ');
        }
    }
}