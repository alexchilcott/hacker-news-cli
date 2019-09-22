using System.Threading.Tasks;

namespace HackerNewsCli.HackerNews
{
    public interface IHackerNewsTopPostRetriever
    {
        /// <summary>
        ///     Retrieves up to <paramref name="numberOfPosts" /> posts from the HackerNews top posts pages. If fewer posts are
        ///     available than have been requested, all available posts will be returned.
        /// </summary>
        /// <param name="numberOfPosts">The number of posts to return</param>
        /// <returns>The top posts that were available from Hacker News</returns>
        Task<Post[]> GetTopPostsAsync(int numberOfPosts);
    }
}