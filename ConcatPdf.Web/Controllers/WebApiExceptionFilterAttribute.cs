using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web;
using System.Web.Http.Filters;

namespace ConcatPdf.Web
{
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private IReadOnlyDictionary<Type, HttpStatusCode> map;
        public WebApiExceptionFilterAttribute()
        {
            map = new ReadOnlyDictionary<Type, HttpStatusCode>(
                new Dictionary<Type, HttpStatusCode>
                {
                    { typeof(ArgumentException), HttpStatusCode.BadRequest },
                    { typeof(ArgumentNullException), HttpStatusCode.BadRequest },
                    { typeof(ArgumentOutOfRangeException), HttpStatusCode.BadRequest },
                    { typeof(SecurityException), HttpStatusCode.Unauthorized },
                    { typeof(NotImplementedException), HttpStatusCode.NotImplemented }
                });
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            if (!(context.Exception is HttpException))
            {
                HttpStatusCode httpStatusCode;
                if (!map.TryGetValue(context.Exception.GetType(), out httpStatusCode))
                {
                    httpStatusCode = HttpStatusCode.InternalServerError;
                }
                context.Response = new HttpResponseMessage(httpStatusCode)
                {
                    Content = new StringContent(context.Exception.Message)
                };
            }
        }
    }
}