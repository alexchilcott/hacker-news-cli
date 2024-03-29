﻿using System;
using System.Threading.Tasks;
using HackerNewsCli.CommandLineArguments;
using HackerNewsCli.Console;
using HackerNewsCli.HackerNews;
using HackerNewsCli.Json;

namespace HackerNewsCli
{
    public class Program
    {
        private readonly IHackerNewsTopPostRetriever _hackerNewsTopPostRetriever;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IOptionsProvider _optionsProvider;
        private readonly IOutputWriter _outputWriter;

        public Program(
            IOptionsProvider optionsProvider,
            IOutputWriter outputWriter,
            IHackerNewsTopPostRetriever hackerNewsTopPostRetriever,
            IJsonSerializer jsonSerializer)
        {
            _optionsProvider = optionsProvider;
            _outputWriter = outputWriter;
            _hackerNewsTopPostRetriever = hackerNewsTopPostRetriever;
            _jsonSerializer = jsonSerializer;
        }

        public async Task<int> RunAsync(string[] args)
        {
            try
            {
                // Parse the command line arguments.
                var optionsParseResult = _optionsProvider.ParseArguments(args);

                if (!optionsParseResult.ParsedSuccessfully)
                {
                    // Display the help text if the arguments could not be parsed.
                    _outputWriter.WriteLine(optionsParseResult.HelpText);
                    return -1;
                }

                var options = optionsParseResult.Options;

                // Retrieve the posts from HackerNews
                var posts = await _hackerNewsTopPostRetriever.GetTopPostsAsync(options.NumberOfPosts);

                // Output the posts to the user.
                var json = _jsonSerializer.Serialize(posts);
                _outputWriter.WriteLine(json);

                return 0;
            }
            catch (Exception ex)
            {
                _outputWriter.WriteLine("An unexpected error has occurred:");
                _outputWriter.WriteLine(ex.Message);
                _outputWriter.WriteLine(ex.StackTrace);
                return -1;
            }
        }
    }
}