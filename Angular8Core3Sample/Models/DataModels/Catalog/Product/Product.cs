using Angular8Core3Sample.Models.DataModels.Catalog.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angular8Core3Sample.Models.DataModels.Catalog.Product
{
    public class Product
    {

        public Product()
        {
            DateCreated = DateTime.Now;
        }

        public int? ProductID { get; set; }

        public IList<ProductName> Names { get; set; }

        public IList<ProductDescription> Descriptions { get; set; }

        public IList<ProductSpecific> Specifics { get; set; }

        public IList<ProductImage> Images { get; set; }

        public IList<ProductPrice> Prices { get; set; }

        public IList<ProductOption> Options { get; set; }

        public Category.Category Category { get; set; }

        public IList<ProductFilter> Filters { get; set; }

        public double? Stock { get; set; }

        public bool? isActive { get; set; }

        public DateTime? DateCreated { get; set; }

    }
}
