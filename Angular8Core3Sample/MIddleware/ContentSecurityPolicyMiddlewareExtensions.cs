using Microsoft.AspNetCore.Builder;
using Angular8Core3Sample.MIddleware.ContentSecurityPolicy;
using System;

namespace Angular8Core3Sample.MIddleware
{
    public static class ContentSecurityPolicyMiddlewareExtensions
    {
        public static IApplicationBuilder UseContentSecurityPolicy(
            this IApplicationBuilder builder, Action<ContentSecurityPolicyBuilder> cspBuilder)
        {
            var newBuilder = new ContentSecurityPolicyBuilder();
            cspBuilder(newBuilder);

            return builder.UseMiddleware<ContentSecurityPolicyMiddleware>(newBuilder);
        }
    }
}
