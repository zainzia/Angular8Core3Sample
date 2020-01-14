using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular8Core3Sample.Models.DataModels.Catalog
{
    public class Country
    {

        public int? CountryID { get; set; }

        public string Name { get; set; }

        public Language Language { get; set; }

        public string Currency { get; set; }

    }
}
