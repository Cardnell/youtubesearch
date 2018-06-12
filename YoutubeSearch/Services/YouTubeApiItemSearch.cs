using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Google;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using YoutubeSearch.Converters;
using YoutubeSearch.Models;

namespace YoutubeSearch.Services
{
    //Given this calls an external API
    //It's a good candidate for asynch coding
    public class YouTubeApiItemSearch : IItemService
    {
        private readonly IConverter<SearchListResponse, ItemSet> _converter;
        private readonly int _maxResults = 10;
        private readonly string _requestPart = "snippet";

        public YouTubeApiItemSearch(IConverter<SearchListResponse, ItemSet> converter)
        {
            _converter = converter;
        }

        public async Task<ItemSet> GetItems(string filter)
        {
            return await GetItems(filter, string.Empty);
        }

        public async Task<ItemSet> GetItems(string filter, string pageToken)
        {
            var youtubeService = YouTubeService();

            var request = youtubeService.Search.List(_requestPart);
            request.Q = filter;
            request.MaxResults = _maxResults;
            if (!string.IsNullOrEmpty(pageToken))
            {
                request.PageToken = pageToken;
            }

            try
            {
                var results = await request.ExecuteAsync();
                return _converter.Convert(results);
            }
            catch (GoogleApiException e)
            {
                //loss of things like stack trace here, but we don't want to expose the youtube stack trace
                //to the end user.  We do however want to return a bad request and this will be handled
                //down the line to do so
                if (e.HttpStatusCode == HttpStatusCode.BadRequest) throw new ArgumentException(e.Message);
                throw;
            }
        }

        private YouTubeService YouTubeService()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = ConfigurationManager.AppSettings["YouTubeApiKey"],
                ApplicationName = GetType().ToString()
            });
            return youtubeService;
        }
    }
}