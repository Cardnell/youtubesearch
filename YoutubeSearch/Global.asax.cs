using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using Google.Apis.YouTube.v3.Data;
using YoutubeSearch.APIModels;
using YoutubeSearch.Converters;
using YoutubeSearch.Models;
using YoutubeSearch.Services;

namespace YoutubeSearch
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var config = GlobalConfiguration.Configuration;

            var builder = new ContainerBuilder();
            builder.RegisterApiControllers();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<YouTubeApiItemSearch>().As<IItemService>().InstancePerRequest();
            builder.RegisterType<ItemSetToYouTubeItemsApiModel>().As<IConverter<ItemSet, YouTubeItemsApiModel>>().InstancePerRequest();
            builder.RegisterType<ThumbnailToThumbnailModel>().As<IConverter<Thumbnail, ThumbnailModel>>().InstancePerRequest();
            builder.RegisterType<SearchResultToItem>().As<IConverter<SearchResult, Item>>().InstancePerRequest();
            builder.RegisterType<SearchListResponseToItemSet>().As<IConverter<SearchListResponse, ItemSet>>().InstancePerRequest();
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
