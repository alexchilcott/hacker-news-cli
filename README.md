# HackerNewsCli

This repository contains my solution for the "Hacker News Scraper Test".

## Assumptions Made

* While using a third party Hacker News API was not allowed, there is an official Hacker News API. I assumed that the purpose of this exercise was to write a tool which scraped the Hacker News HTML rather than one which called their API.

## How To Run

### Prerequisites

This project requires the .NET Core 2.2 SDK installed in order to build and run the application. If this is not the case, download and install the appropriate SDK version for your platform from [The official .NET Core 2.2 Downloads Page](https://dotnet.microsoft.com/download/dotnet-core/2.2).

### Building

Run `build.bat` from the root directory. This will build, test, and publish the project. The final project will be published to the `./bin` directory.

### Running

To run, having run `build.bat`, run (from the `./bin` directory):

```dotnet .\HackerNewsCli.dll --posts <number-of-posts>```

### Building and Running with Docker

The application can be built and run using the supplied `Dockerfile`.

To do so, run 

```docker build . -t hackernews```

followed by

```docker run hackernews --posts 25```

## Libraries Used

* `AngleSharp` and `AngleSharp.IO` are used for the parsing of HTML into a DOM and querying the DOM using CSS selectors.
* `Newtonsoft.Json` was used for JSON serialization when formatting data for STDOUT.
* `Microsoft.Extensions.Configuration.Binder` and `Microsoft.Extensions.Configuration.CommandLine` were used to facilitate parsing of command line arguments.
* `Microsoft.Extensions.DependencyInjection` was included to allow the .NET Core IoC container to be used in order to wire up the components of the application.
* `NUnit`, `NUnit3TestAdapter`, `Microsoft.NET.Test.Sdk` and `Moq` are used to enable unit testing.