namespace HackerNewsCli.CommandLineArguments
{
    public interface IOptionsProvider
    {
        OptionsParseResult ParseArguments(string[] arguments);
    }
}