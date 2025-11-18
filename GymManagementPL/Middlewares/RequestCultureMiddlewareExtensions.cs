using System.Runtime.CompilerServices;

namespace JsonBasedLocalization.web.Middlewares
{
    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleWare>();
        }
    }
}
