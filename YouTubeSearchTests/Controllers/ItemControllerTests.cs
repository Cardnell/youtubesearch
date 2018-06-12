using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using YoutubeSearch.APIModels;
using YoutubeSearch.Controllers;
using YoutubeSearch.Converters;
using YoutubeSearch.Models;
using YoutubeSearch.Services;

namespace YouTubeItemsTests
{
    [TestFixture]
    public class ItemsControllerTests
    {
        [TestCase(".")]
        [TestCase("..")]
        [TestCase("^")]
        [TestCase("&")]
        [TestCase("./test")]
        public async Task PageTokenNotAlphaNumericFails(string token)
        {
            var itemServiceMock = new Mock<IItemService>();
            var filter = "Some values for this Items";
            var pageToken = token;
            itemServiceMock.Setup(x => x.GetItems(filter)).ReturnsAsync(new ItemSet());
            var controller = new ItemsController(itemServiceMock.Object, new ItemSetToYouTubeItemsApiModel());

            var actionResult = await controller.Get(filter, pageToken);
            var result = actionResult as BadRequestErrorMessageResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo("Incorrect page token given"));
        }

        [Test]
        public async Task ItemsWithEmptyFilterReturnsException()
        {
            var itemServiceMock = new Mock<IItemService>();
            var controller = new ItemsController(itemServiceMock.Object, new ItemSetToYouTubeItemsApiModel());

            var actionResult = await controller.Get(string.Empty);
            var result = actionResult as BadRequestErrorMessageResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo("Filter parameter must not be null or empty"));
        }

        [Test]
        public async Task ItemsWithFilterPassesFilterToService()
        {
            var itemServiceMock = new Mock<IItemService>();
            var filter = "Some values for this Items";
            var itemTwo = new Item();
            var itemOne = new Item();
            var itemSet = new ItemSet();
            itemSet.Items.Add(itemOne);
            itemSet.Items.Add(itemTwo);
            itemServiceMock.Setup(x => x.GetItems(filter)).ReturnsAsync(itemSet);
            var controller = new ItemsController(itemServiceMock.Object, new ItemSetToYouTubeItemsApiModel());

            var actionResult = await controller.Get(filter);
            var result = actionResult as OkNegotiatedContentResult<YouTubeItemsApiModel>;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.Not.Null);
            Assert.That(result.Content.Items, Is.EquivalentTo(new List<Item> {itemOne, itemTwo}));
        }

        [Test]
        public async Task ItemsWithNullFilterReturnsException()
        {
            var itemServiceMock = new Mock<IItemService>();
            var controller = new ItemsController(itemServiceMock.Object, new ItemSetToYouTubeItemsApiModel());

            var actionResult = await controller.Get(null);
            var result = actionResult as BadRequestErrorMessageResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo("Filter parameter must not be null or empty"));
        }

        [Test]
        public void ItemsWithoutParameterReturnBadRequest()
        {
            var itemServiceMock = new Mock<IItemService>();
            var controller = new ItemsController(itemServiceMock.Object, new ItemSetToYouTubeItemsApiModel());

            var actionResult = controller.Get();
            var result = actionResult as BadRequestErrorMessageResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Message, Is.EqualTo("Items must be called with a Filter parameter"));
        }


        [Test]
        public async Task ItemsWithPageTokenPassesToService()
        {
            var itemServiceMock = new Mock<IItemService>();
            var filter = "Some values for this Items";
            var pageToken = "Atoken";
            itemServiceMock.Setup(x => x.GetItems(filter, pageToken)).ReturnsAsync(new ItemSet());
            var controller = new ItemsController(itemServiceMock.Object, new ItemSetToYouTubeItemsApiModel());

            await controller.Get(filter, pageToken);

            itemServiceMock.VerifyAll();
        }

        [Test]
        public async Task NonMatchingItemsReturnsEmptyEnumerable()
        {
            var itemServiceMock = new Mock<IItemService>();
            var filter = "No values for this Items";
            itemServiceMock.Setup(x => x.GetItems(filter)).ReturnsAsync(new ItemSet());
            var controller = new ItemsController(itemServiceMock.Object, new ItemSetToYouTubeItemsApiModel());

            var actionResult = await controller.Get(filter);
            var result = actionResult as OkNegotiatedContentResult<YouTubeItemsApiModel>;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.Not.Null);
            Assert.That(result.Content.Items.Count, Is.EqualTo(0));
        }
    }
}