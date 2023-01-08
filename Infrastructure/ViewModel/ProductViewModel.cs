using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel
{
    public class ProductViewModel
    {
        public List<Product>? Products { get; set; }
        public List<LogProduct>? LogProducts { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Brand>? Brands { get; set; }
        public Product? NewProduct { get; set; }

    }
}
