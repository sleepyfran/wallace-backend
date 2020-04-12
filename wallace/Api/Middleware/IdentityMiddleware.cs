using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Wallace.Domain.Identity.Interfaces;

namespace Wallace.Api.Middleware
{
    /// <summary>
    /// Loads the user identity from the claims in the current request and
    /// puts it into the identity container.
    /// </summary>
    public class IdentityMiddleware
    {
        private readonly RequestDelegate _next;

        public IdentityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext httpContext,
            IIdentityLoader identityLoader
        )
        {
            identityLoader.LoadFrom(httpContext.User.Claims);
            await _next(httpContext);
        }
    }
}