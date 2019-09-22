using System;
using System.Threading.Tasks;

namespace HackerNewsCli.Scraping
{
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