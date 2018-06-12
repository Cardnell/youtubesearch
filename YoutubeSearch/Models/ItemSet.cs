using System.Collections.Generic;

namespace YoutubeSearch.Models
{
    public class ItemSet
    {
        public string NextPageToken { get; set; }
        public string PreviousPageToken { get; set; }
        public int TotalResults { get; set; }
        public int ResultsPerPage { get; set; }

        public IList<Item> Items { get; } = new List<Item>();
    }
}