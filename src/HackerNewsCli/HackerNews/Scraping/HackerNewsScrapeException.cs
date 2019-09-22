using System;

namespace HackerNewsCli.HackerNews.Scraping
{
    [Serializable]
    public class HackerNewsScrapeException : Exception
    {
        public HackerNewsScrapeException()
        {
        }

        public HackerNewsScrapeException(string message) : base(message)
        {
        }

        public HackerNewsScrapeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}