using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular8Core3Sample.MIddleware.ContentSecurityPolicy
{
    public class ContentSecurityPolicyBuilder
    {
        public ContentSecurityPolicyOptions Options { get; set; }

        public ContentSecurityPolicyOptionsDirective DefaultSrcsDirective { get; set; }
        public ContentSecurityPolicyOptionsDirective ScriptSrcsDirective { get; set; }
        public ContentSecurityPolicyOptionsDirective ScriptSrcElemsDirective { get; set; }
        public ContentSecurityPolicyOptionsDirective StyleSrcsDirective { get; set; }
        public ContentSecurityPolicyOptionsDirective StyleSrcElemsDirective { get; set; }
        public ContentSecurityPolicyOptionsDirective FrameSrcDirective { get; set; }
        public ContentSecurityPolicyOptionsDirective FontSrcsDirective { get; set; }
        public ContentSecurityPolicyOptionsDirective ConnectSrcsDirective { get; set; }


        public ContentSecurityPolicyBuilder() 
        {
            Options = new ContentSecurityPolicyOptions();

            DefaultSrcsDirective = new ContentSecurityPolicyOptionsDirective();

            ScriptSrcsDirective = new ContentSecurityPolicyOptionsDirective();
            
            StyleSrcsDirective = new ContentSecurityPolicyOptionsDirective();
            
            StyleSrcElemsDirective = new ContentSecurityPolicyOptionsDirective();
            
            FrameSrcDirective = new ContentSecurityPolicyOptionsDirective();
            
            FontSrcsDirective = new ContentSecurityPolicyOptionsDirective();
            
            ConnectSrcsDirective = new ContentSecurityPolicyOptionsDirective();

            ScriptSrcElemsDirective = new ContentSecurityPolicyOptionsDirective();
        }


        public ContentSecurityPolicyOptions Build()
        {
            Options.DefaultSrcs = DefaultSrcsDirective.Sources;
            Options.ConnectSrcs = ConnectSrcsDirective.Sources;
            Options.FontSrcs = FontSrcsDirective.Sources;
            Options.FrameSrc = FrameSrcDirective.Sources;
            Options.ScriptSrcs = ScriptSrcsDirective.Sources;
            Options.StyleSrcElems = StyleSrcElemsDirective.Sources;
            Options.StyleSrcs = StyleSrcsDirective.Sources;
            Options.ScriptSrcElems = ScriptSrcElemsDirective.Sources;

            return Options;
        }

    }
}
