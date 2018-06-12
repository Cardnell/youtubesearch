using Google.Apis.YouTube.v3.Data;
using YoutubeSearch.Models;

namespace YoutubeSearch.Converters
{
    public class SearchListResponseToItemSet : IConverter<SearchListResponse, ItemSet>
    {
        private readonly IConverter<SearchResult, Item> _converter;

        public SearchListResponseToItemSet(IConverter<SearchResult, Item> converter)
        {
            _converter = converter;
        }


        public ItemSet Convert(SearchListResponse model)
        {
            var output = new ItemSet
            {
                NextPageToken = model.NextPageToken,
                PreviousPageToken = model.PrevPageToken,
                ResultsPerPage = model.PageInfo.ResultsPerPage ?? 0,
                TotalResults = model.PageInfo.TotalResults ?? 0
            };


            foreach (var modelItem in model.Items) output.Items.Add(_converter.Convert(modelItem));

            return output;
        }
    }
}