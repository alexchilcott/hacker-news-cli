namespace HackerNewsCli.CommandLineArguments
{
    public interface IOptions
    {
        /// <summary>
        ///     The number of posts requested by the user
        /// </summary>
        int NumberOfPosts { get; }
    }
}