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

        public bool ParsedSuccessfully { get; }
        public IOptions Options { get; }
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