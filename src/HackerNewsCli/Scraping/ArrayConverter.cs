using System.Linq;

namespace HackerNewsCli.Scraping
{
    /// <summary>
    ///     An <see cref="IConverter" /> that, given an <see cref="IConverter" /> for a single item, can convert an array of
    ///     items using the underlying converter.
    /// </summary>
    /// <typeparam name="TIn">The element type of the input array</typeparam>
    /// <typeparam name="TOut">The element type of the output array</typeparam>
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