using System;
using HackerNewsCli.Scraping;

namespace HackerNewsCli.UnitTests.Scraping
{
    public class DelegateConverter<TIn, TOut> : IConverter<TIn, TOut>
    {
        private readonly Func<TIn, TOut> _convertFunc;

        public DelegateConverter(Func<TIn, TOut> convertFunc)
        {
            _convertFunc = convertFunc;
        }

        public TOut Convert(TIn value)
        {
            return _convertFunc(value);
        }
    }
}