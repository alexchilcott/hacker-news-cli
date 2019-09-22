namespace HackerNewsCli.Console
{
    public class StandardOutputWriter : IOutputWriter
    {
        public void WriteLine(string text)
        {
            System.Console.WriteLine(text);
        }
    }
}