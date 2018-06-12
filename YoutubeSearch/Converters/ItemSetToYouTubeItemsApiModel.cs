using YoutubeSearch.APIModels;
using YoutubeSearch.Models;

namespace YoutubeSearch.Converters
{
    public class ItemSetToYouTubeItemsApiModel : IConverter<ItemSet, YouTubeItemsApiModel>
    {
        public YouTubeItemsApiModel Convert(ItemSet model)
        {
            var output = new YouTubeItemsApiModel
            {
                TotalResults = model.TotalResults,
                ResultsPerPage = model.ResultsPerPage
            };

            foreach (var item in model.Items) output.Items.Add(item);

            return output;
        }
    }
}