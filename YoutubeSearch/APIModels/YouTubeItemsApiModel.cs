using System.Collections.Generic;
using YoutubeSearch.Models;

namespace YoutubeSearch.APIModels
{
    public class YouTubeItemsApiModel
    {
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
        public int TotalResults { get; set; }
        public int ResultsPerPage { get; set; }

        public List<Item> Items { get; } = new List<Item>();
    }
}