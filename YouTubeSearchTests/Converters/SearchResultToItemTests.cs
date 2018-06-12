using System;
using Google.Apis.YouTube.v3.Data;
using Moq;
using NUnit.Framework;
using YoutubeSearch.Converters;
using YoutubeSearch.Models;

namespace YouTubeSearchTests.Converters
{
    [TestFixture]
    internal class SearchResultToItemTests
    {
        [TestCase("youtube::video", "videoId")]
        [TestCase("youtube::channel", "channelId")]
        [TestCase("youtube::playlist", "playlistId")]
        public void ConverterAddsCorrectIdToItem(string kind, string expectedId)
        {
            var thumbnailConverterMock = new Mock<IConverter<Thumbnail, ThumbnailModel>>();
            var converter = new SearchResultToItem(thumbnailConverterMock.Object);
            var searchItem = new SearchResult
            {
                Id = new ResourceId
                {
                    VideoId = "videoId",
                    ChannelId = "channelId",
                    PlaylistId = "playlistId",
                    Kind = kind
                },
                Snippet = new SearchResultSnippet {Thumbnails = new ThumbnailDetails()}
            };

            var actual = converter.Convert(searchItem);

            Assert.That(actual.Id, Is.EqualTo(expectedId));
        }

        [TestCase("youtube::video")]
        [TestCase("youtube::channel")]
        [TestCase("youtube::playlist")]
        public void ConverterConvertsKindToType(string kind)
        {
            var thumbnailConverterMock = new Mock<IConverter<Thumbnail, ThumbnailModel>>();
            var converter = new SearchResultToItem(thumbnailConverterMock.Object);
            var searchItem = new SearchResult
            {
                Id = new ResourceId
                {
                    Kind = kind
                },
                Snippet = new SearchResultSnippet {Thumbnails = new ThumbnailDetails()}
            };

            var actual = converter.Convert(searchItem);

            Assert.That(actual.Type, Is.EqualTo(kind));
        }

        [Test]
        public void MapSimpleValues()
        {
            var channelId = "channelId";
            var channelTitle = "ChannelTitle";
            var description = "Description";
            var liveBroadcastContent = "LiveBroadcastContent";
            var publisedAtRaw = "publishedAtRaw";
            var title = "Title";
            var publishedAt = DateTime.Now;
            var thumbnailConverterMock = new Mock<IConverter<Thumbnail, ThumbnailModel>>();
            var converter = new SearchResultToItem(thumbnailConverterMock.Object);
            var searchItem = new SearchResult
            {
                Snippet = new SearchResultSnippet
                {
                    ChannelId = channelId,
                    ChannelTitle = channelTitle,
                    Description = description,
                    LiveBroadcastContent = liveBroadcastContent,
                    PublishedAt = publishedAt,
                    Title = title,
                    Thumbnails = new ThumbnailDetails()
                },
                Id = new ResourceId()
            };

            var actual = converter.Convert(searchItem);

            Assert.That(actual.ChannelId, Is.EqualTo(channelId));
            Assert.That(actual.ChannelTitle, Is.EqualTo(channelTitle));
            Assert.That(actual.Description, Is.EqualTo(description));
            Assert.That(actual.LiveBroadcastContent, Is.EqualTo(liveBroadcastContent));
            Assert.That(actual.PublishedAt.Value.ToString(), Is.EqualTo(publishedAt.ToString()));
            Assert.That(actual.Title, Is.EqualTo(title));
        }
    }
}