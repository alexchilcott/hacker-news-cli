using System;
using HackerNewsCli.CommandLineArguments;
using NUnit.Framework;

namespace HackerNewsCli.UnitTests.CommandLineArguments
{
    public class OptionsProviderTests
    {
        private readonly string StandardHelpText = $"Options:{Environment.NewLine}--posts          How many posts to print. A positive integer <= 100.";

        [TestCase("--posts=1", 1)]
        [TestCase("--posts 1", 1)]
        [TestCase("/posts=1", 1)]
        [TestCase("--posts 100", 100)]
        public void ParseArguments_ValidArguments_ReturnsSuccessResult(string arguments, int expectedNumberOfPosts)
        {
            // Arrange
            var argumentsArray = arguments.Split(" ");
            var optionsProvider = new OptionsProvider();

            // Act
            var result = optionsProvider.ParseArguments(argumentsArray);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParsedSuccessfully, Is.True);
            Assert.That(result.Options.NumberOfPosts, Is.EqualTo(expectedNumberOfPosts));
        }

        [TestCase("-posts=1")]
        [TestCase("--p 1")]
        [TestCase("1")]
        [TestCase("--posts someposts")]
        [TestCase("--posts")]
        public void ParseArguments_InvalidFormat_ReturnsFailureResult(string arguments)
        {
            // Arrange
            var argumentsArray = arguments.Split(" ");
            var optionsProvider = new OptionsProvider();

            // Act
            var result = optionsProvider.ParseArguments(argumentsArray);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParsedSuccessfully, Is.False);
            Assert.That(result.HelpText, Is.EqualTo(StandardHelpText));
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(101)]
        public void ParseArguments_ValidFormatPostCountOutOfRange_ReturnsFailureResult(int postCount)
        {
            // Arrange
            var argumentsArray = new[] {"--posts", postCount.ToString()};
            var optionsProvider = new OptionsProvider();

            // Act
            var result = optionsProvider.ParseArguments(argumentsArray);

            // Assert
            var expectedHelpText = "Number of posts must be between 1 and 100." + Environment.NewLine + Environment.NewLine + StandardHelpText;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParsedSuccessfully, Is.False);
            Assert.That(result.HelpText, Is.EqualTo(expectedHelpText));
        }
    }
}