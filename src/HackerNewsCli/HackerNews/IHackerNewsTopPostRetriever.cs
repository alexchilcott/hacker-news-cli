using System.Threading.Tasks;

namespace HackerNewsCli.HackerNews
{
    public interface IHackerNewsTopPostRetriever
    {
        Task<Post[]> GetTopPostsAsync(int numberOfPosts);
    }
}