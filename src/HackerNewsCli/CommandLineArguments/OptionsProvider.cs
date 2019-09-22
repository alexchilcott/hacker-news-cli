using System;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace HackerNewsCli.CommandLineArguments
{
    public class OptionsProvider : IOptionsProvider
    {
        private string UsageText
        {
            get
            {
                var usageText = new StringBuilder()
                    .AppendLine("Options:")
                    .Append("--posts          How many posts to print. A positive integer <= 100.")
                    .ToString();
                return usageText;
            }
        }

        public OptionsParseResult ParseArguments(string[] arguments)
        {
            // Create an IConfigurationRoot from the provided arguments.
            IConfigurationRoot config;
            try
            {
                config = new ConfigurationBuilder()
                    .AddCommandLine(arguments)
                    .Build();
            }
            catch (FormatException)
            {
                return OptionsParseResult.Failure(UsageText);
            }

            // Read the content of the "posts" argument, if there is one.
            var numberOfPostsStr = config.GetValue<string>("posts");
            if (string.IsNullOrWhiteSpace(numberOfPostsStr) ||
                !int.TryParse(numberOfPostsStr, out var numberOfPosts))
            {
                return OptionsParseResult.Failure(UsageText);
            }

            // The posts argument was a valid integer. Check it is within range.
            if (numberOfPosts <= 0 || numberOfPosts > 100)
            {
                // If the user has supplied an integer that is out of range, the help text will advise them of the valid range.
                var helpText = new StringBuilder()
                    .AppendLine("Number of posts must be between 1 and 100.")
                    .AppendLine()
                    .Append(UsageText)
                    .ToString();
                return OptionsParseResult.Failure(helpText);
            }

            return OptionsParseResult.Success(new Options(numberOfPosts));
        }

        private class Options : IOptions
        {
            public Options(int numberOfPosts)
            {
                NumberOfPosts = numberOfPosts;
            }

            public int NumberOfPosts { get; }
        }
    }
}