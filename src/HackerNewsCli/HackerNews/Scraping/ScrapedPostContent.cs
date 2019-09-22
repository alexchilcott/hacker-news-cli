using System;

namespace HackerNewsCli.HackerNews.Scraping
{
    public class ScrapedPostContent
    {
        public ScrapedPostContent(
            Uri sourcePageUri,
            string titleText,
            string uriText,
            string authorText,
            string pointsText,
            string commentsText,
            string rankText)
        {
            SourcePageUri = sourcePageUri;
            TitleText = titleText;
            UriText = uriText;
            AuthorText = authorText;
            PointsText = pointsText;
            CommentsText = commentsText;
            RankText = rankText;
        }

        public Uri SourcePageUri { get; }
        public string TitleText { get; }
        public string UriText { get; }
        public string AuthorText { get; }
        public string PointsText { get; }
        public string CommentsText { get; }
        public string RankText { get; }
    }
}