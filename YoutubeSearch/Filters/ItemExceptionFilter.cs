using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace YoutubeSearch.Filters
{
    public class ItemExceptionFilter: ExceptionFilterAttribute
    {
        //A deeper dive into the youtube API would probaby have suggested more 
        //excpetions to pick up in this filter
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ArgumentException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(context.Exception.Message)
                };
                return;
            }
            
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(context.Exception.Message)
            };
        }
    }
}