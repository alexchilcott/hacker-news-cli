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
            // In practice the IScraper<Post[]> provided here is a ConvertingScraper<ScrapedPostContent[], Post[]> which uses a
            // HackerNewsPostPageScraper to read ScrapedPostContent[] from a page, and ScrapedPostContentParser to convert those
            // items into Posts.
            _postScraper = postScraper;
        }

        public async Task<Post[]> GetTopPostsAsync(int numberOfPosts)
        {
            if (numberOfPosts <= 0)
            {
                throw new ArgumentException($"{nameof(numberOfPosts)} must be at least 1", nameof(numberOfPosts));
            }

            // Starting from page 1, retrieve posts until we have obtained at least as many as have been requested.
            // Once we have at least as many as have been requested, return the required number of posts.
            // If a page returns 0 posts, assume we have exhausted the posts available and return all the posts we have obtained.
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