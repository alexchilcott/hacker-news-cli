using System.Linq;

namespace HackerNewsCli.Scraping
{
    public class ArrayConverter<TIn, TOut> : IConverter<TIn[], TOut[]>
    {
        private readonly IConverter<TIn, TOut> _singleItemConverter;

        public ArrayConverter(IConverter<TIn, TOut> singleItemConverter)
        {
            _singleItemConverter = singleItemConverter;
        }

        public TOut[] Convert(TIn[] values)
        {
            return values.Select(value => _singleItemConverter.Convert(value)).ToArray();
        }
    }
}