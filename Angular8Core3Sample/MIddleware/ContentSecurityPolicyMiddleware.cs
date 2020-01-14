
using Microsoft.AspNetCore.Http;

using System.Threading.Tasks;

using Angular8Core3Sample.MIddleware.ContentSecurityPolicy;
using System.Text;
using System;

namespace Angular8Core3Sample.MIddleware
{
    public class ContentSecurityPolicyMiddleware
    {
        private const string HEADER = "Content-Security-Policy";

        private readonly RequestDelegate _next;

        private readonly ContentSecurityPolicyBuilder _builder;

        public ContentSecurityPolicyMiddleware(RequestDelegate next, ContentSecurityPolicyBuilder builder)
        {
            _next = next;
            _builder = builder;
        }


        private string GetHeaderValue()
        {
            var src = _builder.Build();

            var headerStringBuilder = new StringBuilder();

            headerStringBuilder.Append("default-src ");
            headerStringBuilder.Append(string.Join(" ", src.DefaultSrcs));
            headerStringBuilder.Append("; ");
            
            headerStringBuilder.Append("connect-src ");
            headerStringBuilder.Append(string.Join(" ", src.ConnectSrcs));
            headerStringBuilder.Append("; ");

            headerStringBuilder.Append("font-src ");
            headerStringBuilder.Append(string.Join(" ", src.FontSrcs));
            headerStringBuilder.Append("; ");

            headerStringBuilder.Append("frame-src ");
            headerStringBuilder.Append(string.Join(" ", src.FrameSrc));
            headerStringBuilder.Append("; ");

            headerStringBuilder.Append("script-src ");
            headerStringBuilder.Append(string.Join(" ", src.ScriptSrcs));
            headerStringBuilder.Append("; ");

            headerStringBuilder.Append("style-src-elem ");
            headerStringBuilder.Append(string.Join(" ", src.StyleSrcElems));
            headerStringBuilder.Append("; ");

            headerStringBuilder.Append("script-src-elem ");
            headerStringBuilder.Append(string.Join(" ", src.ScriptSrcElems));
            headerStringBuilder.Append("; ");

            headerStringBuilder.Append("style-src ");
            headerStringBuilder.Append(string.Join(" ", src.StyleSrcs));
            headerStringBuilder.Append("; ");

            return headerStringBuilder.ToString();
        }


        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add(HEADER, GetHeaderValue());
            await _next(context).ConfigureAwait(false);
        }


    }
}
