using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackerNewsCli.Scraping;

namespace HackerNewsCli.HackerNews.Scraping
{
    public class HackerNewsTopPostScraper : IHackerNewsTopPostRetriever
    {
        private readonly IScraper<Post[]> _postScraper;

        public HackerNewsTopPostScraper(IScraper<Post[]> postScraper)
        {
            _postScraper = postScraper;
        }

        public async Task<Post[]> GetTopPostsAsync(int numberOfPosts)
        {
            if (numberOfPosts <= 0)
            {
                throw new ArgumentException($"{nameof(numberOfPosts)} must be at least 1", nameof(numberOfPosts));
            }

            var retrievedPosts = new List<Post>();
            uint currentPage = 0;
            while (retrievedPosts.Count < numberOfPosts)
            {
                currentPage++;
                var pageUri = new Uri($"https://news.ycombinator.com/news?p={currentPage}");
                var posts = await _postScraper.GetPageModelAsync(pageUri);
                if (posts.Length == 0)
                {
                    return retrievedPosts.ToArray();
                }

                retrievedPosts.AddRange(posts);
            }

            return retrievedPosts.Take(numberOfPosts).ToArray();
        }
    }
}