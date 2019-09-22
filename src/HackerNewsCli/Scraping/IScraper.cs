using System;
using System.Threading.Tasks;

namespace HackerNewsCli.Scraping
{
    public interface IScraper<T>
    {
        Task<T> GetPageModelAsync(Uri uri);
    }
}