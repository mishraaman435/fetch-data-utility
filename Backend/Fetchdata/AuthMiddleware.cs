using Utility;

namespace Fetchdata
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            // 🔓 Public API skip
            if (endpoint?.Metadata.GetMetadata<AllowWithoutAuthAttribute>() != null)
            {
                await _next(context);
                return;
            }

            // 🔐 Auth required
            if (!context.Request.Cookies.TryGetValue("auth_token", out var token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            // 🔍 Future: DB / cache verify
            // if (!TokenService.IsValid(token))
            // {
            //     context.Response.StatusCode = 401;
            //     return;
            // }

            await _next(context);
        }
    }
}
