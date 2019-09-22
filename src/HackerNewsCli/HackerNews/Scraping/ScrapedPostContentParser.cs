using System;
using System.Text.RegularExpressions;
using HackerNewsCli.Scraping;

namespace HackerNewsCli.HackerNews.Scraping
{
    public class ScrapedPostContentParser : IConverter<ScrapedPostContent, Post>
    {
        public Post Convert(ScrapedPostContent scrapedPost)
        {
            var title = ParseTitleText(scrapedPost);
            var uri = ParseUrlText(scrapedPost);
            var author = ParseAuthorText(scrapedPost);
            var points = ParsePointsText(scrapedPost);
            var comments = ParseCommentsText(scrapedPost);
            var rank = ParseRankText(scrapedPost);

            return new Post(title, uri, author, points, comments, rank);
        }

        private int ParseRankText(ScrapedPostContent scrapedPost)
        {
            if (string.IsNullOrWhiteSpace(scrapedPost.RankText))
            {
                throw new HackerNewsScrapeException($"No rank text scraped for a post on {scrapedPost.SourcePageUri}");
            }

            var rankTextRegex = @"^([0-9]+)\.$";
            var match = Regex.Match(scrapedPost.RankText, rankTextRegex);
            if (!match.Success)
            {
                throw new HackerNewsScrapeException($"Rank value did not match expected format for a post on {scrapedPost.SourcePageUri}");
            }

            var rankText = match.Groups[1].ToString();
            if (!int.TryParse(rankText, out var rank))
            {
                throw new HackerNewsScrapeException($"Rank value could not be parsed as an integer for a post on {scrapedPost.SourcePageUri}");
            }

            if (rank < 1)
            {
                throw new HackerNewsScrapeException($"An invalid rank value was found for a post on {scrapedPost.SourcePageUri}");
            }

            return rank;
        }

        private int ParseCommentsText(ScrapedPostContent scrapedPost)
        {
            if (string.IsNullOrWhiteSpace(scrapedPost.CommentsText))
            {
                throw new HackerNewsScrapeException($"No comments text scraped for a post on {scrapedPost.SourcePageUri}");
            }

            if (scrapedPost.CommentsText == "discuss")
            {
                return 0;
            }

            var commentsTextRegex = @"^([0-9]+)\scomments{0,1}$";
            var match = Regex.Match(scrapedPost.CommentsText, commentsTextRegex);
            if (!match.Success)
            {
                throw new HackerNewsScrapeException($"Comments value did not match expected format for a post on {scrapedPost.SourcePageUri}");
            }

            var commentsText = match.Groups[1].ToString();
            if (!int.TryParse(commentsText, out var comments))
            {
                throw new HackerNewsScrapeException($"Comments value could not be parsed as an integer for a post on {scrapedPost.SourcePageUri}");
            }

            if (comments < 1)
            {
                throw new HackerNewsScrapeException($"An invalid comments value was found for a post on {scrapedPost.SourcePageUri}");
            }

            return comments;
        }

        private string ParseAuthorText(ScrapedPostContent scrapedPost)
        {
            if (string.IsNullOrWhiteSpace(scrapedPost.AuthorText))
            {
                throw new HackerNewsScrapeException($"No author text scraped for a post on {scrapedPost.SourcePageUri}");
            }

            if (scrapedPost.AuthorText.Length > 256)
            {
                throw new HackerNewsScrapeException($"The author for a post on {scrapedPost.SourcePageUri} is longer than expected");
            }

            return scrapedPost.AuthorText;
        }

        private int ParsePointsText(ScrapedPostContent scrapedPost)
        {
            if (string.IsNullOrWhiteSpace(scrapedPost.PointsText))
            {
                throw new HackerNewsScrapeException($"No points text scraped for a post on {scrapedPost.SourcePageUri}");
            }

            var pointsTextRegex = @"^([0-9]+)\spoints{0,1}$";
            var match = Regex.Match(scrapedPost.PointsText, pointsTextRegex);
            if (!match.Success)
            {
                throw new HackerNewsScrapeException($"Points value did not match expected format for a post on {scrapedPost.SourcePageUri}");
            }

            var pointsText = match.Groups[1].ToString();
            if (!int.TryParse(pointsText, out var points))
            {
                throw new HackerNewsScrapeException($"Points value could not be parsed as an integer for a post on {scrapedPost.SourcePageUri}");
            }

            if (points < 0)
            {
                throw new HackerNewsScrapeException($"An invalid points value was found for a post on {scrapedPost.SourcePageUri}");
            }

            return points;
        }

        private Uri ParseUrlText(ScrapedPostContent scrapedPost)
        {
            if (string.IsNullOrWhiteSpace(scrapedPost.UriText))
            {
                throw new HackerNewsScrapeException($"No url text scraped for a post on {scrapedPost.SourcePageUri}");
            }

            Uri postUri;

            if (Uri.IsWellFormedUriString(scrapedPost.UriText, UriKind.Absolute))
            {
                postUri = new Uri(scrapedPost.UriText);
            }
            else if (Uri.IsWellFormedUriString(scrapedPost.UriText, UriKind.Relative))
            {
                postUri = new Uri(scrapedPost.SourcePageUri, scrapedPost.UriText);
            }
            else
            {
                throw new HackerNewsScrapeException($"Could not parse {scrapedPost.UriText} into a valid URI");
            }

            return postUri;
        }

        private string ParseTitleText(ScrapedPostContent scrapedPost)
        {
            if (string.IsNullOrWhiteSpace(scrapedPost.TitleText))
            {
                throw new HackerNewsScrapeException($"No title text scraped for a post on {scrapedPost.SourcePageUri}");
            }

            if (scrapedPost.TitleText.Length > 256)
            {
                throw new HackerNewsScrapeException($"The title for a post on {scrapedPost.SourcePageUri} is longer than expected");
            }

            return scrapedPost.TitleText;
        }
    }
}