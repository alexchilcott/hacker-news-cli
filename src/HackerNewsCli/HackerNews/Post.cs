using System;

namespace HackerNewsCli.HackerNews
{
    public class Post
    {
        public Post(string title, Uri uri, string author, int points, int comments, int rank)
        {
            Title = title;
            Uri = uri;
            Author = author;
            Points = points;
            Comments = comments;
            Rank = rank;
        }

        public string Title { get; }

        public Uri Uri { get; }

        public string Author { get; }

        public int Points { get; }

        public int Comments { get; }

        public int Rank { get; }
    }
}