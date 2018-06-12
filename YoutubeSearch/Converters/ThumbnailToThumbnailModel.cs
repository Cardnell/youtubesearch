using Google.Apis.YouTube.v3.Data;
using YoutubeSearch.Models;

namespace YoutubeSearch.Converters
{
    public class ThumbnailToThumbnailModel : IConverter<Thumbnail, ThumbnailModel>
    {
        public ThumbnailModel Convert(Thumbnail model)
        {
            if (model == null)
            {
                return null;
            }

            var output = new ThumbnailModel
            {
                Url = model.Url,
                Height = model.Height,
                Width = model.Width
            };
            return output;
        }
    }
}