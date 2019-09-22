using System;
using System.Threading.Tasks;

namespace HackerNewsCli.Scraping
{
    /// <summary>
    ///     An <see cref="IScraper{TOut}" /> which uses an <see cref="IScraper{TIn}" /> and an
    ///     <see cref="IConverter{TIn,TOut}" /> in order to provide it's output. By composing IScrapers and
    ///     IConverters, scraping 'pipelines' can be built up from different classes providing different parts
    ///     of the full scraping requirements
    /// </summary>
    /// <typeparam name="TIn">The scraped data type provided by the underlying scraper</typeparam>
    /// <typeparam name="TOut">The converted data type for this scraper to provide</typeparam>
    public class ConvertingScraper<TIn, TOut> : IScraper<TOut>
    {
        private readonly IConverter<TIn, TOut> _converter;
        private readonly IScraper<TIn> _underlyingScraper;

        public ConvertingScraper(IScraper<TIn> underlyingScraper, IConverter<TIn, TOut> converter)
        {
            _underlyingScraper = underlyingScraper;
            _converter = converter;
        }

        public async Task<TOut> GetPageModelAsync(Uri uri)
        {
            var inputResult = await _underlyingScraper.GetPageModelAsync(uri);
            return _converter.Convert(inputResult);
        }
    }
}