using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SM.Helper
{
    public class JwtTokenSlidingExpirationMiddleware
    {
        private readonly RequestDelegate next;

        public JwtTokenSlidingExpirationMiddleware(RequestDelegate next)
        {
            this.next = next;
          
        }

        public Task Invoke(HttpContext context)
        {
            // Preflight check 1: did the request come with a token?
            var authorization = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authorization == null || !authorization.ToLower().StartsWith("bearer") || string.IsNullOrWhiteSpace(authorization.Substring(6)))
            {
                // No token on the request
                return next(context);
            }

            // Preflight check 2: did that token pass authentication?
            var claimsPrincipal = context.Request.HttpContext.User;
            if (claimsPrincipal == null || !claimsPrincipal.Identity.IsAuthenticated)
            {
                // Not an authorized request
                return next(context);
            }

            // Extract the claims and put them into a new JWT
            context.Response.Headers.Add("Authorization", JWTService.RefreshToken(context.Request.HttpContext.User.Identity as ClaimsIdentity));

            // Call the next delegate/middleware in the pipeline
            return next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JwtTokenSlidingExpirationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtTokenSlidingExpirationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtTokenSlidingExpirationMiddleware>();
        }
    }
}
