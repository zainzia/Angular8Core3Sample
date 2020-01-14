using System;
using System.Collections.Generic;

namespace Angular8Core3Sample.Models.DataModels.Catalog.Category
{
    
    public class Category
    {

        public Category()
        {
            DateCreated = DateTime.Now;
        }

        public int? CategoryID { get; set; }

        public Category Parent { get; set; }

        public IList<CategoryDescription> Descriptions { get; set; }

        public IList<CategoryImage> Images { get; set; }

        public IList<CategoryName> Names { get; set; }

        public IList<CategoryFilter> Filters { get; set; }

        public IList<CategoryKeyword> Keywords { get; set; }

        public DateTime? DateCreated { get; set; }

    }
}