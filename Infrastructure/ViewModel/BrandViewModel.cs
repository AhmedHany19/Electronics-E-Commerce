using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel
{
    public class BrandViewModel
    {
        public List<Brand>? Brands { get; set; }
        public List<LogBrand>? LogBrands { get; set; }
        public List<Category>? Categories { get; set; }
        public Brand? NewBrand { get; set; }


    }
}
