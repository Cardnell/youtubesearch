using Google.Apis.YouTube.v3.Data;
using YoutubeSearch.Models;

namespace YoutubeSearch.Converters
{
    public class SearchResultToItem : IConverter<SearchResult, Item>
    {
        //CONST needs recompilation of any project using this library
        //In this particular case it seems very unlikely that youtube would
        //change these strings, so I'm using it as apposed to readonly.
        //There are very minor performance improvements.
        //But mostly using switch here is cleaner than ifs, and switch
        //needs consts.
        private const string VideoKindString = "youtube::video";
        private const string ChannelKindString = "youtube::channel";
        private const string PlayListKindString = "youtube::playlist";

        private readonly IConverter<Thumbnail, ThumbnailModel> _thumbnailConverter;

        public SearchResultToItem(IConverter<Thumbnail, ThumbnailModel> thumbnailConverter)
        {
            _thumbnailConverter = thumbnailConverter;
        }

        public Item Convert(SearchResult model)
        {
            var output = new Item
            {
                Type = model.Id.Kind,
                ChannelId = model.Snippet.ChannelId,
                ChannelTitle = model.Snippet.ChannelTitle,
                Description = model.Snippet.Description,
                LiveBroadcastContent = model.Snippet.LiveBroadcastContent,
                PublishedAt = model.Snippet.PublishedAt,
                Title = model.Snippet.Title,
                HighResThumbnail = _thumbnailConverter.Convert(model.Snippet.Thumbnails.High),
                MaxResThumbnail = _thumbnailConverter.Convert(model.Snippet.Thumbnails.Maxres),
                MediumThumbnail = _thumbnailConverter.Convert(model.Snippet.Thumbnails.Medium),
                StandardThumbnail = _thumbnailConverter.Convert(model.Snippet.Thumbnails.Standard)
            };

            switch (model.Id.Kind)
            {
                case VideoKindString:
                    output.Id = model.Id.VideoId;
                    break;
                case ChannelKindString:
                    output.Id = model.Id.ChannelId;
                    break;
                case PlayListKindString:
                    output.Id = model.Id.PlaylistId;
                    break;
            }

            return output;
        }
    }
}