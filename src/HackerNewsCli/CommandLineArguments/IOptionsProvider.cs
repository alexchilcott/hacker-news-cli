namespace HackerNewsCli.CommandLineArguments
{
    public interface IOptionsProvider
    {
        /// <summary>
        ///     Attempts to parse the arguments provided in <paramref name="arguments" />.
        /// </summary>
        /// <param name="arguments">The command line arguments to parse</param>
        /// <returns>
        ///     An <see cref="OptionsParseResult" />. <see cref="OptionsParseResult.ParsedSuccessfully" /> will be true if the
        ///     arguments could be parsed, in which case, <see cref="OptionsParseResult.Options" />
        ///     will also be set. If <see cref="OptionsParseResult.ParsedSuccessfully" />  is false,
        ///     <see cref="OptionsParseResult.HelpText" />  will be set with a usage string, in some cases, some
        ///     additional usage hints.
        /// </returns>
        OptionsParseResult ParseArguments(string[] arguments);
    }
}