using System.Collections.Generic;
using Google.Apis.YouTube.v3.Data;
using Moq;
using NUnit.Framework;
using YoutubeSearch.Converters;
using YoutubeSearch.Models;

namespace YouTubeSearchTests.Converters
{
    [TestFixture]
    public class SearchListResponseToItemSetTests
    {
        [Test]
        public void ConverterTakesResultsPerPageWhereGiven()
        {
            var converterMock = new Mock<IConverter<SearchResult, Item>>();

            var converter = new SearchListResponseToItemSet(converterMock.Object);
            var searchListResponse = new SearchListResponse()
            {
                PageInfo = new PageInfo()
                {
                    ResultsPerPage = 25,
                    TotalResults = 15
                },
                Items = new List<SearchResult>()
            };

            var actual = converter.Convert(searchListResponse);

            Assert.That(actual.ResultsPerPage, Is.EqualTo(25));
        }

        [Test]
        public void ConverterAssignsZeroToResultsPerPageWhenNull()
        {
            var converterMock = new Mock<IConverter<SearchResult, Item>>();

            var converter = new SearchListResponseToItemSet(converterMock.Object);
            var searchListResponse = new SearchListResponse()
            {
                PageInfo = new PageInfo()
                {
                    ResultsPerPage = null,
                    TotalResults = 15
                },
                Items = new List<SearchResult>()
            };

            var actual = converter.Convert(searchListResponse);

            Assert.That(actual.ResultsPerPage, Is.EqualTo(0));
        }

        [Test]
        public void ConverterTakesTotalResultsWhereGiven()
        {
            var converterMock = new Mock<IConverter<SearchResult, Item>>();

            var converter = new SearchListResponseToItemSet(converterMock.Object);
            var searchListResponse = new SearchListResponse()
            {
                PageInfo = new PageInfo()
                {
                    ResultsPerPage = 25,
                    TotalResults = 15
                },
                Items = new List<SearchResult>()
            };

            var actual = converter.Convert(searchListResponse);

            Assert.That(actual.TotalResults, Is.EqualTo(15));
        }

        [Test]
        public void ConverterAssignsZeroToTotalResultsWhenNull()
        {
            var converterMock = new Mock<IConverter<SearchResult, Item>>();

            var converter = new SearchListResponseToItemSet(converterMock.Object);
            var searchListResponse = new SearchListResponse()
            {
                PageInfo = new PageInfo()
                {
                    ResultsPerPage = 25,
                    TotalResults = null
                },
                Items = new List<SearchResult>()
            };

            var actual = converter.Convert(searchListResponse);

            Assert.That(actual.TotalResults, Is.EqualTo(0));
        }
    }
}