using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular8Core3Sample.Services.Models
{
    public class TreeTableFormat
    {

        public Dictionary<string, dynamic> data;
        public List<TreeTableFormat> children;  

        public TreeTableFormat()
        {
            data = new Dictionary<string, dynamic>();
            //children = new List<TreeTableFormat>();
        }
    }
}
