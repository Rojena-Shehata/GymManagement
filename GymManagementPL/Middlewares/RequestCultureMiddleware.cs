using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace JsonBasedLocalization.web.Middlewares
{
    public class RequestCultureMiddleWare
    {
        private readonly RequestDelegate _requestDelegate;

        public RequestCultureMiddleWare(RequestDelegate  requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public  async  Task InvokeAsync(HttpContext httpContext)
        {
            var currentLanguage = httpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            var browserLanguage = httpContext.Request.Headers["Accept-Language"].ToString()[..2];//substring first 2 chars of browser language
            if(string.IsNullOrEmpty(currentLanguage))
            {
                var  culture=string.Empty;
                switch (browserLanguage)
                {
                    case "ar":
                        culture = "ar-EG";
                        break;

                    default:
                        culture = "en-US";
                        break;
                }
                var requestCulture = new RequestCulture(culture, culture);
                httpContext.Features.Set<IRequestCultureFeature>(new RequestCultureFeature(requestCulture,null));
                CultureInfo.CurrentCulture=new CultureInfo(culture);
                CultureInfo.CurrentUICulture=new CultureInfo(culture);
            }
            await _requestDelegate(httpContext);
        }
    }
}
