using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Angular8Core3Sample.Services
{
    public class RazorViewsLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context) { }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[]
            {
                "/Views/Admin/{1}/{0}.cshtml",
            }.Union(viewLocations); // add `.Union(viewLocations)` to add default locations
        }
    }
}
