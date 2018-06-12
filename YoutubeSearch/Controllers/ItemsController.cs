using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using YoutubeSearch.APIModels;
using YoutubeSearch.Converters;
using YoutubeSearch.Filters;
using YoutubeSearch.Models;
using YoutubeSearch.Services;

namespace YoutubeSearch.Controllers
{
    [ItemExceptionFilter]
    public class ItemsController : ApiController
    {
        private readonly IConverter<ItemSet, YouTubeItemsApiModel> _converter;
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService, IConverter<ItemSet, YouTubeItemsApiModel> converter)
        {
            _itemService = itemService;
            _converter = converter;
        }

        public IHttpActionResult Get()
        {
            return BadRequest("Items must be called with a Filter parameter");
        }

        public async Task<IHttpActionResult> Get(string filter)
        {
            return await Get(filter, string.Empty);
        }

        public async Task<IHttpActionResult> Get(string filter, string pageToken)
        {
            if (string.IsNullOrEmpty(filter)) return BadRequest("Filter parameter must not be null or empty");

            if (!CheckAlphaNumeric(pageToken)) return BadRequest("Incorrect page token given");

            var results = string.IsNullOrEmpty(pageToken)
                ? await _itemService.GetItems(filter)
                : await _itemService.GetItems(filter, pageToken);
            var output = ParseResults(results, filter);
            return Ok(output);
        }

        private YouTubeItemsApiModel ParseResults(ItemSet results, string filter)
        {
            var model = _converter.Convert(results);

            model.NextPageUrl = ValidateAndCreateUrl(results, filter, results.NextPageToken);
            model.PreviousPageUrl = ValidateAndCreateUrl(results, filter, results.PreviousPageToken);

            return model;
        }

        private string ValidateAndCreateUrl(ItemSet results, string filter, string token)
        {
            return CheckAlphaNumeric(token) && !string.IsNullOrEmpty(token)
                ? Url.Link("DefaultApi", new {Controller = "Items", filter, pageToken = token})
                : string.Empty;
        }

        private static bool CheckAlphaNumeric(string token)
        {
            return string.IsNullOrEmpty(token) || token.All(char.IsLetterOrDigit);
        }
    }
}