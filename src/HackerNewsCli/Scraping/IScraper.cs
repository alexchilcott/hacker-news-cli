using System;
using System.Threading.Tasks;

namespace HackerNewsCli.Scraping
{
    /// <summary>
    ///     A simple interface for scraping a typed model of a page from it's URI.
    /// </summary>
    /// <typeparam name="T">The model which represents the scraped data from a page</typeparam>
    public interface IScraper<T>
    {
        /// <summary>
        ///     Scrapes the data from the provided URI, and returns a model representing the scraped data from the page.
        /// </summary>
        /// <param name="uri">The URI of the page to scrape</param>
        /// <returns>A model representing the scraped data</returns>
        Task<T> GetPageModelAsync(Uri uri);
    }
}