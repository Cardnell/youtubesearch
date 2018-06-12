using System;

namespace YoutubeSearch.Models
{
    public class Item
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string ChannelId { get; set; }

        public string ChannelTitle { get; set; }

        public string Description { get; set; }

        public string LiveBroadcastContent { get; set; }

        public DateTime? PublishedAt { get; set; }

        //Could make a collection here.  I was in two minds
        public ThumbnailModel StandardThumbnail { get; set; }

        public ThumbnailModel HighResThumbnail { get; set; }

        public ThumbnailModel MaxResThumbnail { get; set; }

        public ThumbnailModel MediumThumbnail { get; set; }

        public string Title { get; set; }
    }
}