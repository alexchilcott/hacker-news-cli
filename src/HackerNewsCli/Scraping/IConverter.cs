namespace HackerNewsCli.Scraping
{
    public interface IConverter<in TIn, out TOut>
    {
        TOut Convert(TIn value);
    }
}