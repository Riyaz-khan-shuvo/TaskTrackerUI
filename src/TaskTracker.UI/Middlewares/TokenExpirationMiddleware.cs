using TaskTracker.UI.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace TaskTracker.UI.Middlewares
{
    public class TokenExpirationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenExpirationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            // Skip static files
            //if (path.StartsWith("/css") || path.StartsWith("/js") || path.StartsWith("/images") || path.StartsWith("/lib"))
            //{
            //    await _next(context);
            //    return;
            //}

            //var endpoint = context.GetEndpoint();
            //if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            //{
            //    await _next(context);
            //    return;
            //}

            var endpoint = context.GetEndpoint();

            // Skip if endpoint is null (like for static files, favicon, etc.)
            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            if (endpoint.DisplayName?.Contains("StaticFile") == true ||
                endpoint.DisplayName?.Contains("Razor") == true ||
                endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                await _next(context);
                return;
            }



            var token = context.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                context.Session.Clear();
                context.Response.Redirect("/Account/Login");
                return;
            }

            var jwtInfo = JwtHelper.DecodeJwt(token);

            if (jwtInfo == null || jwtInfo.Expiration <= DateTime.UtcNow)
            {
                context.Session.Clear();
                context.Response.Redirect("/Account/Login");
                return;
            }

            // ✅ Handle Access Denied (403) response
            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                // Prevent further response writing
                context.Response.Clear();
                context.Response.Redirect("/Home/AccessDenied");
                return;
            }

            await _next(context);
        }



    }
    public static class TokenExpirationMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenExpirationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenExpirationMiddleware>();
        }
    }
}
