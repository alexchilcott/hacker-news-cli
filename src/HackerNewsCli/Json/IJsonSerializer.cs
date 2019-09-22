namespace HackerNewsCli.Json
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T item);
    }
}