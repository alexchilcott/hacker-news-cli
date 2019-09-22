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

            var numberOfPostsStr = config.GetValue<string>("posts");
            if (string.IsNullOrWhiteSpace(numberOfPostsStr) ||
                !int.TryParse(numberOfPostsStr, out var numberOfPosts))
            {
                return OptionsParseResult.Failure(UsageText);
            }

            if (numberOfPosts <= 0 || numberOfPosts > 100)
            {
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