using System;
namespace Bds.TechTest.Models
{
    public class SearchResult
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string UrlView { get; set; }
        public string Paragraph { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SearchResult result &&
                   Title == result.Title &&
                   Url == result.Url;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Url);
        }
    }
}
