using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular8Core3Sample.MIddleware.ContentSecurityPolicy
{
    public class ContentSecurityPolicyOptions
    {
        internal List<string> DefaultSrcs { get; set; }
        internal List<string> ScriptSrcs { get; set; }
        internal List<string> ScriptSrcElems { get; set; }
        internal List<string> StyleSrcs { get; set; }
        internal List<string> StyleSrcElems { get; set;  }
        internal List<string> FrameSrc { get; set;  }
        internal List<string> FontSrcs { get; set;  }
        internal List<string> ConnectSrcs { get; set;  }
    }
}
