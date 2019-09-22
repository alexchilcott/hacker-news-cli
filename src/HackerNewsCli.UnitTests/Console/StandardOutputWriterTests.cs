using System;
using System.IO;
using HackerNewsCli.Console;
using NUnit.Framework;

namespace HackerNewsCli.UnitTests.Console
{
    public class StandardOutputWriterTests
    {
        private StringWriter _fakeConsoleStream;
        private TextWriter _originalOutput;
        private StandardOutputWriter _outputWriter;

        [SetUp]
        public void SetUp()
        {
            _fakeConsoleStream = new StringWriter();
            _originalOutput = System.Console.Out;
            System.Console.SetOut(_fakeConsoleStream);
            _outputWriter = new StandardOutputWriter();
        }

        [TearDown]
        public void TearDown()
        {
            System.Console.SetOut(_originalOutput);
            _fakeConsoleStream.Dispose();
        }

        [Test]
        public void WriteLine_WritesToConsoleStandardOutput()
        {
            // Arrange
            var text = "Hello world";

            // Act
            _outputWriter.WriteLine(text);

            // Assert
            var stdOutContent = _fakeConsoleStream.ToString();
            Assert.That(stdOutContent, Is.EqualTo($"{text}{Environment.NewLine}"));
        }
    }
}