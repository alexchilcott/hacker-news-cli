namespace HackerNewsCli.CommandLineArguments
{
    public class OptionsParseResult
    {
        private OptionsParseResult(bool parsedSuccessfully, IOptions options, string helpText)
        {
            ParsedSuccessfully = parsedSuccessfully;
            Options = options;
            HelpText = helpText;
        }

        /// <summary>
        ///     True if the options could be successfully parsed. If this is true, <see cref="Options" /> will be set. Otherwise,
        ///     <see cref="HelpText" /> will be set.
        /// </summary>
        public bool ParsedSuccessfully { get; }


        /// <summary>
        ///     The parsed options from the provided command line arguments. Only available if <see cref="ParsedSuccessfully" /> is
        ///     true.
        /// </summary>
        public IOptions Options { get; }

        /// <summary>
        ///     A user-friendly string to display to advise of correct program usage. Only available if
        ///     <see cref="ParsedSuccessfully" /> is false.
        /// </summary>
        public string HelpText { get; }

        public static OptionsParseResult Success(IOptions options)
        {
            return new OptionsParseResult(true, options, null);
        }

        public static OptionsParseResult Failure(string helpText)
        {
            return new OptionsParseResult(false, null, helpText);
        }
    }
}