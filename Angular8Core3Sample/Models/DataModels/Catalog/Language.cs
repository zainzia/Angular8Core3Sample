using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Angular8Core3Sample.Models.DataModels.Catalog
{
    [Serializable]
    public class Language
    {
        public int LanguageID { get; set; }

        public string Name { get; set; }

        public string Culture { get; set; }
    }
}