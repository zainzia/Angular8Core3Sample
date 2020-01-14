
using System.Collections.Generic;

namespace Angular8Core3Sample.MIddleware.ContentSecurityPolicy
{
    public class ContentSecurityPolicyOptionsDirective
    {

        internal List<string> Sources { get; set; }

        internal ContentSecurityPolicyOptionsDirective() 
        {
            Sources = new List<string>();
        }

        public ContentSecurityPolicyOptionsDirective AllowSelf()
        {
            return Allow("'self'");
        }

        public ContentSecurityPolicyOptionsDirective AllowUnsafeEval()
        {
            return Allow("'unsafe-eval'");
        }

        public ContentSecurityPolicyOptionsDirective AllowUnsafeInline()
        {
            return Allow("'unsafe-inline'");
        }

        public ContentSecurityPolicyOptionsDirective Allow(string source)
        {
            Sources.Add(source);
            return this;
        }
    }
}
