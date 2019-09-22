namespace HackerNewsCli.Console
{
    /// <summary>
    ///     Represents a simple interface for writing text to an output device.
    /// </summary>
    public interface IOutputWriter
    {
        /// <summary>
        ///     Writes the provided text to the output, followed by a line break.
        /// </summary>
        void WriteLine(string text);
    }
}